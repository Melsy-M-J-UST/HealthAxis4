using HealthAxis3.Shared.Models.Dtos.DoctorDtos;

namespace HealthAxis3.API.Service
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllAsync();
        Task<DoctorDto> GetByIdAsync(int id);
        Task<List<DoctorDto>> GetByNameAsync(string name);
        Task<List<DoctorDto>> GetBySpecialisationAsync(string specialisation);
        Task<DoctorDto> AddAsync(DoctorDto entity);
        Task<DoctorUpdateDto> UpdateAsync(int id, DoctorUpdateDto entity);
        Task<List<string>> GetAvailableSlots(int doctorId, DateTime date);
        Task<DoctorUpdateDto?> DeactivateDoctorAsync(int id);
    }
}
