using apbd11.Models;

namespace apbd11.Services;

public class DbService:IDbService
{
    public Task<Prescription> GetPrescription(int id)
    {
        throw new NotImplementedException();
    }

    public Task AddNewPrescription(Prescription prescription)
    {
        throw new NotImplementedException();
    }
}