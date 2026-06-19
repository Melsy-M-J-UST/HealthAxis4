using HealthAxis3.API.Models;
using HealthAxis3.API.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthAxis3.API.Service.Implementation
{
    public class AuthService(UserManager<ApplicationUser> userManager, IConfiguration config) : IAuthService
    {
        public async Task<(bool Success, string Message, string token, int ExpiresIn)> Login(LoginDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {

                return (false, "Invalid Email or Password", string.Empty, 0);
            }
            var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {

                return (false, "Invalid Email or Password", string.Empty, 0);

            }
            var token = await GenerateToken(user);
            var expiry = int.Parse(config.GetSection("jwt")["AccessTokenExpirationMinutes"]!);
            return (true, "User LoggedIn Successfully", token, expiry);
        }

        public async Task<(bool Success, string Message, string UserId)> Register(RegisterDto request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return (false, "Password Do not Match", string.Empty);
            }
            if (request.Role != "Admin" && request.Role != "Patient" && request.Role != "Doctor")
            {
                return (false, "Invalid Role", string.Empty);
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(equals => equals.Description));
                return (false, errors, string.Empty);
            }
            await userManager.AddToRoleAsync(user, request.Role);
            return (true, "user Registered Succesfully", user.Id);

        }
        private async Task<string> GenerateToken(ApplicationUser user)
        {
            var jwtSettings = config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim> {
                    new(JwtRegisteredClaimNames.Sub, user.Id),
                    //new(JwtRegisteredClaimNames.Email, user.Email),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(ClaimTypes.NameIdentifier, user.Id)
                };

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            }

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

            }
            bool isParsed = int.TryParse(jwtSettings["AccessTokenExpirationMinutes"], out int expirationMinutes);
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials

                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
