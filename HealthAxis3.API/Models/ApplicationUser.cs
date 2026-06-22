using Microsoft.AspNetCore.Identity;

namespace HealthAxis3.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsFirstLogin { get; set; }=true;
    }
}
