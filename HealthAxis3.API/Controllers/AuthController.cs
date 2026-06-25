using HealthAxis3.API.Models;
using HealthAxis3.Shared.Models;
using HealthAxis3.Shared.Models.Dtos;
using HealthAxis3.API.Service;
using HealthAxis3.API.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService service) : ControllerBase
    {
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var (success, message, userId) = await service.Register(request);
            if (!success)
            {
                return BadRequest(new { message });
            }
            return Ok(new { message, userId });
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var (Success, message, response, ExpiresIn) = await service.Login(request);
            if (!Success)
            {
                return Unauthorized(new { message });
            }
            if (response == null)
            {
                return Unauthorized(new { message = "Token generation failed" });
            }
            
            return Ok(response);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (request == null)
                return BadRequest(new { Message = "Invalid request" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get user email from token/claims
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            if (request.OldPassword == request.NewPassword)
            {
                return BadRequest(new
                {
                    Message = "New password cannot be same as old password"
                });
            }

            var (Success, Message, Errors) = await service.ChangePassword(email, request.OldPassword, request.NewPassword);

            if (!Success)
            {
                return BadRequest(new
                {
                    Message,
                    Errors
                });
            }

            return Ok(new
            {
                Message
            });
        }
    }
}