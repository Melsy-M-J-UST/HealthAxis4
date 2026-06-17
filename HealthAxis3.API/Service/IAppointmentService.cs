using HealthAxis3.API.Models.Dtos;

namespace HealthAxis3.API.Service
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAllAsync();
        Task<AppointmentDto> GetByIdAsync(int id);
        Task<AppointmentDto> AddAsync(AppointmentDto entity);
    }
}
