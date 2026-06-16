using AutoMapper;
using HealthAxis3.API.Models;
using HealthAxis3.API.Models.Dtos;
using HealthAxis3.API.Repository;

namespace HealthAxis3.API.Service.Implementation
{
    public class DoctorService(IDoctorRepository repository, IMapper mapper) : IDoctorService
    {
        public async Task<DoctorDto> AddAsync(DoctorDto entity)
        {
            var doctor = mapper.Map<Doctor>(entity);
            var savedEntity = repository.CreateAsync(doctor);
            return mapper.Map<DoctorDto>(savedEntity);
        }

        //public async Task<DoctorDto> DeactivateAsync(int id)
        //{
        //    var deleted = await repository.DeleteAsync(id);
        //    return mapper.Map<DoctorDto>(deleted);
        //}

        public async Task<List<DoctorDto>> GetAllAsync()
        {
            return mapper.Map<List<DoctorDto>>(await repository.GetAllAsync());
        }

        public async Task<DoctorDto> GetByIdAsync(int id)
        {
            return mapper.Map<DoctorDto>(await repository.GetByIdAsync(id));
        }

        public async Task<DoctorDto> UpdateAsync(int id, DoctorDto entity)
        {
            var doctor = mapper.Map<Doctor>(entity);
            doctor.DoctorId = id;
            var updated = await repository.UpdateAsync(id, doctor);
            return mapper.Map<DoctorDto>(updated);

        }
    }
}
