using HealthAxis3.API.Models.Dtos;

namespace HealthAxis3.API.Service
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllAsync();
        Task<DoctorDto> GetByIdAsync(int id);
        Task<DoctorDto> AddAsync(DoctorDto entity);
        Task<DoctorDto> UpdateAsync(int id, DoctorDto entity);
        //Task<DoctorDto> DeactivateAsync(int id);
    }
}
