using HealthAxis3.API.Models;

namespace HealthAxis3.API.Repository
{
    public interface IHealthRecordRepository : IRepository<HealthRecord>
    {
        Task<List<HealthRecord>> GetByDoctorIdAsync(int id, CancellationToken ct = default);
        Task<List<HealthRecord>> GetByPatientIdAsync(int id, CancellationToken ct = default);
    }
}
