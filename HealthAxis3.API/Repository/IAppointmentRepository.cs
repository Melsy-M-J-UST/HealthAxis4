using HealthAxis3.API.Models;

namespace HealthAxis3.API.Repository
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<List<Appointment>> GetByDoctorIdAsync(int id, CancellationToken ct = default);
        Task<List<Appointment>> GetByPatientIdAsync(int id, CancellationToken ct = default);
    }
}
