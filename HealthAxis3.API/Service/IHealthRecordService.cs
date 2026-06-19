using HealthAxis3.API.Models.Dtos.HealthrecordDto;

namespace HealthAxis3.API.Service
{
    public interface IHealthRecordService
    {
        Task<List<HealthRecordDto>> GetAllAsync();
        Task<HealthRecordDto> GetByIdAsync(int id);
        Task<HealthRecordDto> AddAsync(HealthRecordDto entity);
        Task<List<HealthRecordDto>> GetByDoctorIdAsync(int id);
        Task<List<HealthRecordDto>> GetByPatientIdAsync(int id);
    }
}
