using AutoMapper;
using HealthAxis3.API.Models;
using HealthAxis3.Shared.Models.Dtos.DoctorDtos;
using HealthAxis3.API.Repository;

namespace HealthAxis3.API.Service.Implementation
{
    public class DoctorService(IDoctorRepository repository, IMapper mapper) : IDoctorService
    {
        public async Task<DoctorDto> AddAsync(DoctorDto entity)
        {
            var doctor = mapper.Map<Doctor>(entity);
            var savedEntity = await repository.CreateAsync(doctor);
            return mapper.Map<DoctorDto>(savedEntity);
        }

        public async Task<List<DoctorDto>> GetAllAsync()
        {
            return mapper.Map<List<DoctorDto>>(await repository.GetAllAsync());
        }

        public async Task<DoctorDto> GetByIdAsync(int id)
        {
            return mapper.Map<DoctorDto>(await repository.GetByIdAsync(id));
        }
        public async Task<List<DoctorDto>> GetByNameAsync(string name)
        {
            return mapper.Map<List<DoctorDto>>(await repository.GetByNameAsync(name));
        }
        public async Task<List<DoctorDto>> GetBySpecialisationAsync(string specialisation)
        {
            return mapper.Map<List<DoctorDto>>(await repository.GetBySpecialisationAsync(specialisation));
        }

        public async Task<DoctorUpdateDto> UpdateAsync(int id, DoctorUpdateDto entity)
        {
            var doctor = mapper.Map<Doctor>(entity);
            doctor.DoctorId = id;
            var updated = await repository.UpdateAsync(id, doctor);
            return mapper.Map<DoctorUpdateDto>(updated);

        }
        public async Task<List<string>> GetAvailableSlots(int doctorId, DateTime date)
        {
            return await repository.GetDoctorAvailability(doctorId, date);
        }

        public async Task<DoctorUpdateDto?> DeactivateDoctorAsync(int id)
        {
            var deactivated = await repository.DeactivateAsync(id);
            return mapper.Map<DoctorUpdateDto>(deactivated);
        }
    }
}
