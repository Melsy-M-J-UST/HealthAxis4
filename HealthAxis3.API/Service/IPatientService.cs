using HealthAxis3.Shared.Models.Dtos.PatientDtos;

namespace HealthAxis3.API.Service
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync();
        Task<PatientDto> GetByIdAsync(int id);
        Task<PatientDto> AddAsync(PatientCreateDto entity);
        Task<PatientDto> UpdateAsync(int id, PatientDto entity);
        Task<List<PatientDto>> GetByNameAsync(string name);
        Task<List<PatientDto>> GetByPhoneAsync(string phone);
        Task<PatientDto?> DeactivatePatientAsync(int id);
        Task<PatientUpdateDto?> UpdateStatusAsync(int id, bool isActive);
    }
}
