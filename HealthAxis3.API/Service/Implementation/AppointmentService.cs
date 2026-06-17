using AutoMapper;
using HealthAxis3.API.Models.Dtos;
using HealthAxis3.API.Repository;

namespace HealthAxis3.API.Service.Implementation
{
    public class AppointmentService(IAppointmentRepository repository, IMapper mapper) : IAppointmentService
    {
        public Task<AppointmentDto> AddAsync(AppointmentDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<AppointmentDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AppointmentDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
