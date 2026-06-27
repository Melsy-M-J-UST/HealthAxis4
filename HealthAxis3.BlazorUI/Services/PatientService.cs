using HealthAxis3.Shared.Models.Dtos.PatientDtos;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;
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
        public async Task<bool> UpdatePatientAsync(int id, bool isActive)
        {
            var response = await _http.PutAsJsonAsync($"api/patient/{id}/update", isActive );
            return response.IsSuccessStatusCode;
        }
        public async Task ToggleStatus(int id)
        {
            var response = await _http.PutAsync($"api/Admin/Patients/{id}/status", null);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to update status");
        }
    }
}
