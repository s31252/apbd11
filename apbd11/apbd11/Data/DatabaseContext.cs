using apbd11.Models;
using Microsoft.EntityFrameworkCore;

namespace apbd11.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected DatabaseContext()
    {
        
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PrescriptionMedicament>()
            .HasKey(pm => new { pm.IdMedicament, pm.IdPrescription });

        modelBuilder.Entity<PrescriptionMedicament>()
            .HasOne<Prescription>()
            .WithMany(p => p.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdPrescription);

        modelBuilder.Entity<PrescriptionMedicament>()
            .HasOne<Medicament>()
            .WithMany()
            .HasForeignKey(pm => pm.IdMedicament);
        
        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Patient)
            .WithMany()
            .HasForeignKey(p => p.IdPatient);
        
        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Doctor)
            .WithMany()
            .HasForeignKey(p => p.IdDoctor);
        
        modelBuilder.Entity<Doctor>().HasData(new List<Doctor>
        {
            new Doctor { IdDoctor = 1, FirstName = "AAA", LastName = "AAA", Email = "AAA@mail.com" },
            new Doctor { IdDoctor = 2, FirstName = "BBB", LastName = "BBB", Email = "BBB@mail.com" },
        });

        modelBuilder.Entity<Patient>().HasData(new List<Patient>
        {
            new Patient { IdPatient = 1, FirstName = "Jan", LastName = "Kowalski", BirthDate = DateTime.Now },
            new Patient { IdPatient = 2, FirstName = "Kamil", LastName = "Nowak", BirthDate = DateTime.Now }
        });

        modelBuilder.Entity<Medicament>().HasData(new List<Medicament>
        {
            new Medicament { IdMedicament = 1, Name = "AAA", Description = "Some desc...", Type = "AAA" }
        });
    }

    
    
    
    
}