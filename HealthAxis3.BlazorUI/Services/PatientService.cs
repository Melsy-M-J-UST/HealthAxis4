using HealthAxis3.Shared.Models.Dtos.PatientDtos;
using System.Net.Http.Json;
namespace HealthAxis3.BlazorUI.Services
{
    public class PatientService
    {
        private readonly HttpClient _http;
        public PatientService(HttpClient http)
        {
            _http = http;
        }
        public async Task<List<PatientDto>> GetPatientsAsync()
        {
            var result = await _http.GetFromJsonAsync<List<PatientDto>>("api/patient");
            return result ?? new List<PatientDto>();
        }
        public async Task<bool> UpdatePatientStatusAsync(int id, bool isActive)
        {
            var response = await _http.PutAsJsonAsync(
                $"api/patient/{id}/status",
                new { IsActive = isActive }
            );
            return response.IsSuccessStatusCode;
        }
    }
}
