using AutoMapper;
using HealthAxis3.API.Models.Dtos;
using HealthAxis3.API.Repository;

namespace HealthAxis3.API.Service.Implementation
{
    public class HealthRecordService(IHealthRecordRepository repository, IMapper mapper) : IHealthRecordService
    {
        public Task<HealthRecordDto> AddAsync(HealthRecordDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<HealthRecordDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<HealthRecordDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
