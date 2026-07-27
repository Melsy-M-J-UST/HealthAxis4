using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxis3.API.Data
{
    public static class UserSeeder
    {
        [ExcludeFromCodeCoverage]
        public static async Task SeedAdminUserAsync(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            const string email = "Admin@healthaxis.com";
            const string password = "Admin@123";

            // Ensure Admin role exists
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Check if admin user already exists
            var existingUser = await userManager.FindByEmailAsync(email);

            if (existingUser == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}