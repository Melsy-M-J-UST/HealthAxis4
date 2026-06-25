using HealthAxis3.API.Models;

namespace HealthAxis3.API.Repository
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task<List<Doctor>> GetByNameAsync(string name, CancellationToken ct = default);
        Task<List<Doctor>> GetBySpecialisationAsync(string specialisation, CancellationToken ct = default);
        Task<Doctor?> DeactivateAsync(int id, CancellationToken ct = default);
        Task<List<string>> GetDoctorAvailability(int doctorId, DateTime date, CancellationToken ct= default);
    }
}
