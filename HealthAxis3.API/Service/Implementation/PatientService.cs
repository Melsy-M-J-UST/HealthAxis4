using AutoMapper;
using HealthAxis3.API.Models;
using HealthAxis3.API.Repository;
using HealthAxis3.Shared.Models.Dtos.PatientDtos;

namespace HealthAxis3.API.Service.Implementation
{
    public class PatientService(IPatientRepository repository, IMapper mapper) : IPatientService
    {
        public async Task<PatientDto> AddAsync(PatientDto entity)
        {
            var patient = mapper.Map<Patient>(entity);
            var savedEntity = await repository.CreateAsync(patient);
            return mapper.Map<PatientDto>(savedEntity);
        }

        public async Task<PatientDto?> DeactivatePatientAsync(int id)
        {
            var deactivated = await repository.DeactivateAsync(id);
                return mapper.Map<PatientDto>(deactivated);
        }

        public async Task<List<PatientDto>> GetAllAsync()
        {
            return mapper.Map<List<PatientDto>>(await repository.GetAllAsync());
        }

        public async Task<PatientDto> GetByIdAsync(int id)
        {
            return mapper.Map<PatientDto>(await repository.GetByIdAsync(id));
        }

        public async Task<List<PatientDto>> GetByNameAsync(string name)
        {
            return mapper.Map<List<PatientDto>>(await repository.GetByNameAsync(name));
        }

        public async Task<List<PatientDto>> GetByPhoneAsync(string phone)
        {
            return mapper.Map<List<PatientDto>>(await repository.GetByPhoneAsync(phone));
        }

        public async Task<PatientDto> UpdateAsync(int id, PatientDto entity)
        {
            var patient = mapper.Map<Patient>(entity);
            patient.PatientId = id;
            var updated = await repository.UpdateAsync(id, patient);
            return mapper.Map<PatientDto>(updated);

        }
        public async Task<PatientUpdateDto?> UpdateStatusAsync(int id, bool isActive)
        {
            var patient = await repository.GetByIdAsync(id);

            if (patient == null)
                return null;

            patient.IsActive = isActive;

            var updated = await repository.UpdateAsync(id, patient);

            return mapper.Map<PatientUpdateDto>(updated);
        }
    }
}
