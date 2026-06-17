using HealthAxis3.API.Models.Dtos.DoctorDto;

namespace HealthAxis3.API.Service
{
    public interface IAdminService
    {
        Task<DoctorDto> AddAsync(DoctorDto entity);
        Task<DoctorUpdateDto> UpdateAsync(int id, DoctorUpdateDto entity);
        Task<DoctorUpdateDto> DeactivateAsync(int id);
    }
}
