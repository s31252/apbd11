using apbd11.DTOs;
using apbd11.Models;

namespace apbd11.Services;

public interface IDbService
{
    Task<PatientRequestDto> GetPrescription(int id);
    Task AddNewPrescription(PrescriptionRequestDto prescription);
}