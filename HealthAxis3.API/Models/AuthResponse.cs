using System.Diagnostics.CodeAnalysis;

namespace HealthAxis3.API.Models
{
    [ExcludeFromCodeCoverage]
    public class AuthResponse
    {
        public string Accesstoken { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
        public string Role { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;


    }
}
