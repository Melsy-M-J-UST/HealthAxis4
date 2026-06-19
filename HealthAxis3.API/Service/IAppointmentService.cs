using HealthAxis3.API.Models;
using HealthAxis3.API.Models.Dtos.AppointmentDto;

namespace HealthAxis3.API.Service
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAllAsync();
        Task<AppointmentDto> GetByIdAsync(int id);
        Task<AppointmentDto> AddAsync(AppointmentDto entity);
        Task<List<AppointmentDto>> GetByDoctorIdAsync(int id, CancellationToken ct = default);
        Task<List<AppointmentDto>> GetByPatientIdAsync(int id, CancellationToken ct = default);
        Task<string> UpdateAppointmentStatus(int id, string status, string? reason = null);
        Task<string> DeleteAppointment(int id);
    }
}
