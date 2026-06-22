using HealthAxis3.Shared.Models.Dtos.AppointmentDtos;
using HealthAxis3.API.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController(IAppointmentService service) : ControllerBase
    {
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] AppointmentDto entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var result = await service.AddAsync(entity);
                if (result == null) return NotFound();
                else return CreatedAtAction("GetById", new { result.AppointmentId }, result);
            }
        }
        [HttpPut("{id}/status")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Patient, Doctor")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status, [FromQuery] string? reason)
        {
            var result = await service.UpdateAppointmentStatus(id, status, reason);

            if (result == "REDIRECT_TO_HEALTH_RECORD")
            {
                return Ok(new
                {
                    message = "Completed successfully",
                    redirectUrl = $"/HealthRecord/Create?appointmentId={id}"
                });
            }

            return Ok(new { message = result });
        }

        // ✅ Delete
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await service.DeleteAppointment(id);
            return Ok(new { message = result });
        }
    }

}

