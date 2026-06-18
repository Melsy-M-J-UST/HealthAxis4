using AutoMapper;
using HealthAxis3.API.Models;
using HealthAxis3.API.Models.Dtos.AppointmentDto;
using HealthAxis3.API.Models.Dtos.PatientDto;
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

        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            return mapper.Map<List<AppointmentDto>>(await repository.GetAllAsync());
        }

        public async Task<AppointmentDto> GetByIdAsync(int id)
        {
            return mapper.Map<AppointmentDto>(await repository.GetByIdAsync(id));
        }
    }
}
