using AutoMapper;
using HealthAxis3.API.Models;
using HealthAxis3.API.Repository;
using HealthAxis3.Shared.Models.Dtos.DoctorDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace HealthAxis3.API.Service.Implementation
{
    public class DoctorService(IDoctorRepository repository, IMapper mapper, UserManager<ApplicationUser> userManager, IDistributedCache cache) : IDoctorService
    {
        private const string doctorsCacheKey = "doctors:all";
        private static readonly DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            SlidingExpiration = TimeSpan.FromMinutes(10) 
        };
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

            var doctorDto= mapper.Map<DoctorDto>(saved);
            await cache.RemoveAsync(doctorsCacheKey);
            return doctorDto;
        }

        public async Task<List<DoctorDto>> GetAllAsync()
        {
            var cached = await cache.GetStringAsync(doctorsCacheKey);
            if (!string.IsNullOrEmpty(cached))
            {
            var doctorList= JsonSerializer.Deserialize<List<DoctorDto>>(cached) ?? new List<DoctorDto>();
                if (doctorList != null) return doctorList;
            }
            var doctors = mapper.Map<List<DoctorDto>>(await repository.GetAllAsync());
            await cache.SetStringAsync(doctorsCacheKey, JsonSerializer.Serialize(doctors), cacheOptions);
            return doctors;
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
            var updatedDto = mapper.Map<DoctorUpdateDto>(updated);
            await cache.RemoveAsync(doctorsCacheKey);
            return updatedDto;
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
