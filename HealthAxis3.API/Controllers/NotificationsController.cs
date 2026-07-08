using HealthAxis3.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController(AppDbContext dbContext) : ControllerBase
    {
        private readonly AppDbContext _dbContext = dbContext;

        [HttpGet("{doctorId}")]
        public async Task<IActionResult> Get(int doctorId)
        {
            var notifications =
                await _dbContext.Notifications
                    .Where(x => x.DoctorId == doctorId)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();

            return Ok(notifications);
        }
    }
}
