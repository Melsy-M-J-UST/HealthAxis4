using HealthAxis3.API.Models;

namespace HealthAxis3.API.Repository
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<List<Appointment>> GetByDoctorIdAsync(int id, CancellationToken ct = default);
        Task<List<Appointment>> GetByPatientIdAsync(int id, CancellationToken ct = default);
        Task<Appointment?> GetWithDetailsAsync(int id, CancellationToken ct = default);
        Task<List<Appointment>> GetAllWithDetailsAsync(CancellationToken ct = default);
        Task<List<Appointment>> GetExpiredCancelledAsync(CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);

    }
}
