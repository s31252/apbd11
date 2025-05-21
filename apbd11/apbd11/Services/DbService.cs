using apbd11.Data;
using apbd11.DTOs;
using apbd11.Models;
using Microsoft.EntityFrameworkCore;

namespace apbd11.Services;

public class DbService:IDbService
{
    private readonly DatabaseContext _context;

    public DbService(DatabaseContext context)
    {
        _context = context;
    }
    
public async Task<PatientRequestDto> GetPrescription(int id)
{
    var patient = await _context.Patients.FirstOrDefaultAsync(p => p.IdPatient == id);
    if (patient == null)
        throw new ArgumentException("Pacjent nie istnieje.");

    var prescriptions = await _context.Prescriptions
        .Where(p => p.IdPatient == id)
        .ToListAsync();

    var prescriptionIds = prescriptions.Select(p => p.IdPrescription).ToList();

    var prescriptionMedicaments = await _context.PrescriptionMedicaments
        .Where(pm => prescriptionIds.Contains(pm.IdPrescription))
        .ToListAsync();

    var medicamentIds = prescriptionMedicaments.Select(pm => pm.IdMedicament).Distinct().ToList();

    var medicaments = await _context.Medicaments
        .Where(m => medicamentIds.Contains(m.IdMedicament))
        .ToListAsync();

    var doctors = await _context.Doctors.ToListAsync();

    var result = new PatientRequestDto
    {
        IdPatient = patient.IdPatient,
        FirstName = patient.FirstName,
        LastName = patient.LastName,
        BirthDate = patient.BirthDate,
        Prescriptions = prescriptions
            .OrderBy(p => p.DueDate)
            .Select(p => new PrescriptionDto
            {
                IdPrescription = p.IdPrescription,
                Date = p.Date,
                DueDate = p.DueDate,
                Doctor = doctors
                    .Where(d => d.IdDoctor == p.IdDoctor)
                    .Select(d => new DoctorDto
                    {
                        IdDoctor = d.IdDoctor,
                        FirstName = d.FirstName
                    })
                    .FirstOrDefault(),
                Medicaments = prescriptionMedicaments
                    .Where(pm => pm.IdPrescription == p.IdPrescription)
                    .Select(pm =>
                    {
                        var medicament = medicaments.FirstOrDefault(m => m.IdMedicament == pm.IdMedicament);
                        return new PatientMedicamentDto
                        {
                            IdMedicament = pm.IdMedicament,
                            Name = medicament?.Name,
                            Dose = pm.Dose,
                            Details = pm.Details
                        };
                    }).ToList()
            })
            .ToList()
    };

    return result;
}





    public async Task AddNewPrescription(PrescriptionRequestDto dto)
{
    if (dto.Medicaments == null || dto.Medicaments.Count == 0)
        throw new ArgumentException("No medicaments provided");

    if (dto.Medicaments.Count > 10)
        throw new ArgumentException("Prescriptions must have 10 medicaments");

    if (dto.DueDate < dto.Date)
        throw new ArgumentException("DueDate must be greater than or equal to date");
    
    var doctor = await _context.Doctors.FindAsync(dto.IdDoctor);
    if (doctor == null)
        throw new ArgumentException("Doctor not found");
    
    var patient = _context.Patients.FirstOrDefault(p =>
        p.FirstName == dto.Patient.FirstName &&
        p.LastName == dto.Patient.LastName &&
        p.BirthDate == dto.Patient.BirthDate);

    if (patient == null)
    {
        patient = new Patient
        {
            FirstName = dto.Patient.FirstName,
            LastName = dto.Patient.LastName,
            BirthDate = dto.Patient.BirthDate
        };
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();
    }
    
    var prescription = new Prescription
    {
        Date = dto.Date,
        DueDate = dto.DueDate,
        IdDoctor = dto.IdDoctor,
        IdPatient = patient.IdPatient,
        PrescriptionMedicaments = new List<PrescriptionMedicament>()
    };
    
    foreach (var medDto in dto.Medicaments)
    {
        var medicament = await _context.Medicaments.FindAsync(medDto.IdMedicament);
        if (medicament == null)
            throw new ArgumentException($"Medicament with ID {medDto.IdMedicament} not found.");

        prescription.PrescriptionMedicaments.Add(new PrescriptionMedicament
        {
            IdMedicament = medDto.IdMedicament,
            Dose = medDto.Dose,
            Details = medDto.Details
        });
    }

    await _context.Prescriptions.AddAsync(prescription);
    await _context.SaveChangesAsync();
}

}