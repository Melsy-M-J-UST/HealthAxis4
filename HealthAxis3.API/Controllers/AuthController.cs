using HealthAxis3.API.Models;
using HealthAxis3.API.Models.Dtos;
using HealthAxis3.API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService service) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            var (success, message, userId) = await service.Register(request);
            if (!success)
            {
                return BadRequest(new { message });
            }
            return Ok(new { message, userId });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var (Success, message, token, ExpiresIn) = await service.Login(request);
            if (!Success)
            {
                return Unauthorized(new { message });
            }
            AuthResponse response = new AuthResponse
            {
                Accesstoken = token,
                Message = message,
                ExpiresIn = ExpiresIn
            };
            return Ok(response);
        }
    }
}
