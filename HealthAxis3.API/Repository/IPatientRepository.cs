using HealthAxis3.API.Models;

namespace HealthAxis3.API.Repository
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<List<Patient>> GetByNameAsync(string name, CancellationToken ct = default);
        Task<List<Patient>> GetByPhoneAsync(string phone, CancellationToken ct = default);
        Task<Patient?> DeactivateAsync(int id, CancellationToken ct = default);
    }
}
