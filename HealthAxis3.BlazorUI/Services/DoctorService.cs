using HealthAxis3.Shared.Models.Dtos.DoctorDtos;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace HealthAxis3.BlazorUI.Services
{
    public class DoctorService
    {
        private readonly HttpClient http;

        public DoctorService(HttpClient http)
        {
            this.http = http;
        }

        public async Task<List<DoctorDto>> GetDoctorsAsync()
        {
            var doctors=await http.GetFromJsonAsync<List<DoctorDto>>("api/Admin/Doctors");
            if (doctors == null)
            {
                return new List<DoctorDto>();
            }
            return doctors;
        }
        public async Task ToggleStatus(int id)
        {
            var response = await http.PutAsync($"api/Admin/Doctors/{id}/status", null);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to update status");
        }

        public async Task<bool> UpdateDoctorAsync(int id, DoctorDto doctor)
        {
            var response = await http.PutAsJsonAsync($"api/Admin/Doctors/{id}", doctor);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddDoctorAsync(DoctorDto doctor)
        {
            var response = await http.PostAsJsonAsync(
                "api/Admin/Doctors", doctor);

            return response.IsSuccessStatusCode;
        }
        public async Task<List<DoctorDto>> GetBySpecialisationAsync(string spec)
        {
            var result = await http.GetFromJsonAsync<List<DoctorDto>>(
                $"api/Doctor/search/specialisation?spec={spec}");

            return result ?? new List<DoctorDto>();
        }


    }
}
