using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace HealthAxis3.Shared.Models
{
    [ExcludeFromCodeCoverage]
    public class AuthResponse
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
        public string Role { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int? PatientId { get; set; }
        public int? DoctorId { get; set; }

    }
}
