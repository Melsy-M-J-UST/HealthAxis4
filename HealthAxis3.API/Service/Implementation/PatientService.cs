using AutoMapper;
using HealthAxis3.API.Models;
using HealthAxis3.API.Models.Dtos;
using HealthAxis3.API.Repository;
using HealthAxis3.API.Repository.Implementation;

namespace HealthAxis3.API.Service.Implementation
{
    public class PatientService(IPatientRepository repository, IMapper mapper) : IPatientService
    {
        public async Task<PatientDto> AddAsync(PatientDto entity)
        {
            var patient = mapper.Map<Patient>(entity);
            var savedEntity = repository.CreateAsync(patient);
            return mapper.Map<PatientDto>(savedEntity);
        }

        //public async Task<PatientDto> DeleteAsync(int id)
        //{
        //    var deleted = await repository.DeleteAsync(id);
        //    return mapper.Map<PatientDto>(deleted);
        //}

        public async Task<List<PatientDto>> GetAllAsync()
        {
            return mapper.Map<List<PatientDto>>(await repository.GetAllAsync());
        }

        public async Task<PatientDto> GetByIdAsync(int id)
        {
            return mapper.Map<PatientDto>(await repository.GetByIdAsync(id));
        }

        public async Task<PatientDto> UpdateAsync(int id, PatientDto entity)
        {
            var patient = mapper.Map<Patient>(entity);
            patient.PatientId = id;
            var updated = await repository.UpdateAsync(id, patient);
            return mapper.Map<PatientDto>(updated);

        }
    }
}
