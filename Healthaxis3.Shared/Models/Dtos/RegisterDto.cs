using System.Diagnostics.CodeAnalysis;

namespace HealthAxis3.Shared.Models.Dtos
{
    [ExcludeFromCodeCoverage]
    public class RegisterDto
    {
        public string PatientName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? InsuranceId { get; set; }

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Role { get; set; } = "Patient";
    }
}
