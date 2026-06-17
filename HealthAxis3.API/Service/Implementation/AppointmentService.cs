using AutoMapper;
using HealthAxis3.API.Models;
using HealthAxis3.API.Models.Dtos.AppointmentDto;
using HealthAxis3.API.Repository;

namespace HealthAxis3.API.Service.Implementation
{
    public class AppointmentService(IAppointmentRepository repository, IMapper mapper) : IAppointmentService
    {
        public async Task<AppointmentDto> AddAsync(AppointmentDto entity)
        {
            var appointment = mapper.Map<Appointment>(entity);
            var savedEntity = repository.CreateAsync(appointment);
            return mapper.Map<AppointmentDto>(savedEntity);
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
