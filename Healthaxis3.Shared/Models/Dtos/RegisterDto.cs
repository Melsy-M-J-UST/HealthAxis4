using System.Diagnostics.CodeAnalysis;

namespace HealthAxis3.Shared.Models.Dtos
{
    [ExcludeFromCodeCoverage]
    public class RegisterDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Role { get; set; } = "Patient";
    }
}
