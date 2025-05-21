using apbd11.Models;

namespace apbd11.Services;

public interface IDbService
{
    Task<Prescription> GetPrescription(int id);
    Task AddNewPrescription(Prescription prescription);
}