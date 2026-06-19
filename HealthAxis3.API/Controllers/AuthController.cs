using HealthAxis3.API.Models;
using HealthAxis3.API.Models.Dtos;
using HealthAxis3.API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService service, UserManager<ApplicationUser> userManager) : ControllerBase
    {
        [HttpPost("register")]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var (Success, message, token, ExpiresIn) = await service.Login(request);
            if (!Success)
            {
                return Unauthorized(new { message });
            }
            AuthResponse response = new()
            {
                Accesstoken = token,
                Message = message,
                ExpiresIn = ExpiresIn
            };
            return Ok(response);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            // 1. Validate confirm password
            if (request.NewPassword != request.ConfirmPassword)
            {
                return BadRequest("New password and confirm password do not match.");
            }

            // 2. Get logged-in user
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // 3. Check old password + update password
            if (request.OldPassword == request.NewPassword)
            {
                return BadRequest("New password cannot be same as old password.");
            }
            var result = await userManager.ChangePasswordAsync(
                user,
                request.OldPassword,
                request.NewPassword
            );

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Password changed successfully.");
        }
    }
}