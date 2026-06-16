using AutoMapper;
using HealthAxis3.API.Models;
using HealthAxis3.API.Models.Dtos;
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
            throw new NotImplementedException();
        }

        public async Task<HealthRecordDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
