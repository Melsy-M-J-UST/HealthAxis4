using HealthAxis3.API.Models.Dtos;

namespace HealthAxis3.API.Service
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, string UserId)> Register(RegisterDto request);
        Task<(bool Success, string Message, string token, int ExpiresIn)> Login(LoginDto login);
    }
}
