using HealthAxis3.API.Data;
using HealthAxis3.API.Models;
using HealthAxis3.Shared.Models;
using HealthAxis3.Shared.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthAxis3.API.Service.Implementation
{
    public class AuthService(UserManager<ApplicationUser> userManager, AppDbContext context, IConfiguration config) : IAuthService
    {
        public async Task<(bool Success, string Message, AuthResponse? Data, int ExpiresIn)> Login(LoginDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return (false, "Invalid credentials", null, 0);

            var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
                return (false, "Invalid credentials", null, 0);

            var roles = await userManager.GetRolesAsync(user);
            var doctor = context.Doctors.FirstOrDefault(d => d.UserId == user.Id);
            if (user.IsFirstLogin && roles.Any(r => r == "Doctor"))
            {

                var tokens = await GenerateToken(user);

                var responses = new AuthResponse
                {
                    AccessToken = tokens,
                    Role = roles.FirstOrDefault() ?? "",
                    UserId = user.Id,
                    Message = "FirstLogin",
                    DoctorId = doctor?.DoctorId
                };

                return (true, "FirstLogin", responses, 0);

            }

            var token = await GenerateToken(user);

            var role = roles.FirstOrDefault() ?? "";

            var expiry = int.Parse(config["Jwt:AccessTokenExpirationMinutes"]!);

            var patient = context.Patients
                .FirstOrDefault(p => p.UserId == user.Id);
            var response = new AuthResponse
            {
                AccessToken = token,
                Role = role,
                UserId = user.Id,
                PatientId = patient?.PatientId
            };

            return (true, "User Logged in Successfully", response, expiry);
        }

        public async Task<(bool Success, string Message, string UserId)> Register(RegisterDto request)
        {
            if (request.Password != request.ConfirmPassword)
                return (false, "Password Do Not Match", "");

            if (request.Role == "Doctor")
                return (false, "Doctors must be created by Admin", "");

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                IsFirstLogin = true
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return (false, "Error creating user", "");

            await userManager.AddToRoleAsync(user, request.Role);
            if (request.Role == "Patient")
            {
                var patient = new Patient
                {
                    UserId = user.Id,
                    PatientName = request.PatientName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    DateOfBirth = request.DateOfBirth,
                    Gender = request.Gender,
                    InsuranceId = request.InsuranceId
                };

                context.Patients.Add(patient);
                await context.SaveChangesAsync();
            }

            return (true, "User Registered Successfully", user.Id);

        }

        public async Task<(bool Success, string Message)> CreateDoctorUser(string email)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                IsFirstLogin = true
            };

            var result = await userManager.CreateAsync(user, "Doctor@123");

            if (!result.Succeeded)
                return (false, "Doctor creation failed");

            await userManager.AddToRoleAsync(user, "Doctor");

            return (true, "Doctor user created");
        }

        public async Task<(bool Success, string Message, IEnumerable<string>? Errors)> ChangePassword( string email, string oldPassword, string newPassword)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
                return (false, "User not found", null);

            var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (!result.Succeeded)
            {
                return (false, "Password change failed", result.Errors.Select(e => e.Description));
            }

            user.IsFirstLogin = false;

            var updateResult = await userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return (false, "Failed to update user", updateResult.Errors.Select(e => e.Description));
            }

            return (true, "Password changed successfully", null);
        }

        private async Task<string> GenerateToken(ApplicationUser user)
        {
            var jwtSettings = config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new(ClaimTypes.NameIdentifier, user.Id)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(jwtSettings["AccessTokenExpirationMinutes"]!)
                ),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
