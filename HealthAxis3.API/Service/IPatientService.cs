using HealthAxis3.API.Models;
using HealthAxis3.API.Models.Dtos.PatientDto;

namespace HealthAxis3.API.Service
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync();
        Task<PatientDto> GetByIdAsync(int id);
        Task<PatientDto> AddAsync(PatientDto entity);
        Task<PatientDto> UpdateAsync(int id, PatientDto entity);
        Task<List<PatientDto>> GetByNameAsync(string name);
        Task<List<PatientDto>> GetByPhoneAsync(string phone);
        Task<PatientDto?> DeactivateAsync(int id);
    }
}
