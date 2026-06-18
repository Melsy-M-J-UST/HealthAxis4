using AutoMapper;
using HealthAxis3.API.Models.Dtos.DoctorDto;
using HealthAxis3.API.Models.Dtos.PatientDto;
using HealthAxis3.API.Repository.Implementation;

namespace HealthAxis3.API.Service
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllAsync();
        Task<DoctorDto> GetByIdAsync(int id);
        Task<DoctorDto> AddAsync(DoctorDto entity);
        Task<DoctorUpdateDto> UpdateAsync(int id, DoctorUpdateDto entity);
        Task<List<string>> GetAvailableSlots(int doctorId, DateTime date);
        Task<DoctorUpdateDto?> DeactivateDoctorAsync(int id);
    }
}
