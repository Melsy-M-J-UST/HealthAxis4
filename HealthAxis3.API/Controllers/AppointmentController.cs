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
    public class AppointmentController(IAppointmentService service, ILogger<AppointmentController> logger) : ControllerBase
    {
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Doctor, Patient")]
        public async Task<IActionResult> GetAll()
        {
            var result = await service.GetAllAsync();
            logger.LogInformation("Retrieved all appointments successfully.");
            return Ok(result);
        }
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Patient, Doctor")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await service.GetByIdAsync(id);
            if (result == null)
            {
                logger.LogWarning("Appointment with ID {id} not found.", id);
                return NotFound();
            }
            logger.LogInformation(" Details of {id} received",id);
                return Ok(result);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Patient")]
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

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await service.DeleteAppointment(id);
            return Ok(new { message = result });
        }

        [HttpGet("patient/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Patient, Doctor")]
        public async Task<IActionResult> GetByPatientId([FromRoute] int id)
        {
            var result = await service.GetByPatientIdAsync(id);
            return Ok(result);
        }

        [HttpGet("doctor/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Doctor")]
        public async Task<IActionResult> GetByDoctorId([FromRoute] int id)
        {
            var result = await service.GetByDoctorIdAsync(id);
            return Ok(result);
        }
    }
}
