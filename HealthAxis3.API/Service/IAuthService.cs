using HealthAxis3.API.Models;
using HealthAxis3.Shared.Models.Dtos;

namespace HealthAxis3.API.Service
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, string UserId)> Register(RegisterDto request);
        Task<(bool Success, string Message, AuthResponse? Data, int ExpiresIn)> Login(LoginDto request);
        Task<(bool Success, string Message)> CreateDoctorUser(string email);
        Task<(bool Success, string Message, IEnumerable<string>? Errors)> ChangePassword(string email, string oldPassword, string newPassword);
    }
}
