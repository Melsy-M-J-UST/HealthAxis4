using AutoMapper;
using HealthAxis3.API.Models;
using HealthAxis3.Shared.Models.Dtos.HealthrecordDtos;
using HealthAxis3.API.Repository;

namespace HealthAxis3.API.Service.Implementation
{
    public class HealthRecordService(IHealthRecordRepository repository, IMapper mapper) : IHealthRecordService
    {
        public async Task<HealthRecordDto> AddAsync(HealthRecordDto entity)
        {
            var healthRecord = mapper.Map<HealthRecord>(entity);
            var savedEntity = await repository.CreateAsync(healthRecord);
            return mapper.Map<HealthRecordDto>(savedEntity);
        }

        public async Task<List<HealthRecordDto>> GetAllAsync()
        {
            return mapper.Map<List<HealthRecordDto>>(await repository.GetAllAsync());
        }

        public async Task<HealthRecordDto> GetByIdAsync(int id)
        {
            return mapper.Map<HealthRecordDto>(await repository.GetByIdAsync(id));
        }
        public async Task<List<HealthRecordDto>> GetByDoctorIdAsync(int id)
        {
            return mapper.Map<List<HealthRecordDto>>(await repository.GetByDoctorIdAsync(id));
        }
        public async Task<List<HealthRecordDto>> GetByPatientIdAsync(int id)
        {
            return mapper.Map<List<HealthRecordDto>>(await repository.GetByPatientIdAsync(id));
        }
        public async Task<HealthRecordDto?> GetByAppointmentIdAsync(int id)
        {
            var record = await repository.GetByAppointmentIdAsync(id);
            if (record == null)
                return null;
            return mapper.Map<HealthRecordDto>(record);
        }
    }
}
