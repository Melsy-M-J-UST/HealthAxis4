using HealthAxis3.API.Models.Dtos.DoctorDto;

namespace HealthAxis3.API.Service
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllAsync();
        Task<DoctorDto> GetByIdAsync(int id);
    }
}
