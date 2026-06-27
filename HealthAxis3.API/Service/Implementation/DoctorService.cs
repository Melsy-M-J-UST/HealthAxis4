using AutoMapper;
using HealthAxis3.API.Models;
using HealthAxis3.API.Repository;
using HealthAxis3.Shared.Models.Dtos.DoctorDtos;
using Microsoft.AspNetCore.Identity;

namespace HealthAxis3.API.Service.Implementation
{
    public class DoctorService(IDoctorRepository repository, IMapper mapper, UserManager<ApplicationUser> userManager) : IDoctorService
    {
        public async Task<DoctorDto> AddAsync(DoctorDto entity)
        {
            var doctor = mapper.Map<Doctor>(entity);

            var saved = await repository.CreateAsync(doctor);

            var email = GenerateDoctorEmail(saved.DoctorName);

            var user = new ApplicationUser
            {
                Email = email,
                UserName = email
            };

            await userManager.CreateAsync(user, "Doctor@123");
            await userManager.AddToRoleAsync(user, "Doctor");

            return mapper.Map<DoctorDto>(saved);
        }

        public async Task<List<DoctorDto>> GetAllAsync()
        {
            return mapper.Map<List<DoctorDto>>(await repository.GetAllAsync());
        }

        public async Task<DoctorDto> GetByIdAsync(int id)
        {
            return mapper.Map<DoctorDto>(await repository.GetByIdAsync(id));
        }
        public async Task<List<DoctorDto>> GetByNameAsync(string name)
        {
            return mapper.Map<List<DoctorDto>>(await repository.GetByNameAsync(name));
        }
        public async Task<List<DoctorDto>> GetBySpecialisationAsync(string specialisation)
        {
            return mapper.Map<List<DoctorDto>>(await repository.GetBySpecialisationAsync(specialisation));
        }

        public async Task<DoctorUpdateDto> UpdateAsync(int id, DoctorUpdateDto entity)
        {
            var doctor = mapper.Map<Doctor>(entity);
            doctor.DoctorId = id;
            var updated = await repository.UpdateAsync(id, doctor);
            return mapper.Map<DoctorUpdateDto>(updated);

        }
        public async Task<List<string>> GetAvailableSlots(int doctorId, DateTime date)
        {
            return await repository.GetDoctorAvailability(doctorId, date);
        }

        public async Task<DoctorUpdateDto?> DeactivateDoctorAsync(int id)
        {
            var deactivated = await repository.DeactivateAsync(id);
            return mapper.Map<DoctorUpdateDto>(deactivated);
        }
        private string GenerateDoctorEmail(string name)
        {
            name = name.ToLower();

            var parts = name.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 1)
            {
                var first = parts[0];
                var rest = string.Join("", parts.Skip(1));
                return $"{first}.{rest}@healthaxis.com";
            }

            return $"{name}@healthaxis.com";
        }
    }
}
