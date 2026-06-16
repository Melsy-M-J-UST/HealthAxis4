using HealthAxis3.API.Models.Dtos;

namespace HealthAxis3.API.Service
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync();
        Task<PatientDto> GetByIdAsync(int id);
        Task<PatientDto> AddAsync(PatientDto entity);
        Task<PatientDto> UpdateAsync(int id, PatientDto entity);
        Task<PatientDto> DeleteAsync(int id);
    }
}
