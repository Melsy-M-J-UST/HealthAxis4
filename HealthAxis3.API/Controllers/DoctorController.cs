using HealthAxis3.API.Models.Dtos.DoctorDto;
using HealthAxis3.API.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController(IDoctorService service) : ControllerBase
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
        [HttpGet("{name}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetByName([FromRoute] string name)
        {
            var result = await service.GetByNameAsync(name);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }
        [HttpGet("{specialisation}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetBySpecialisation([FromRoute] string specialisation)
        {
            var result = await service.GetBySpecialisationAsync(specialisation);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }
        [HttpGet("doctors/{doctorId}/availability")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Patient")]
        public async Task<IActionResult> GetDoctorAvailability(int doctorId, DateTime date)
        {
            var slots = await service.GetAvailableSlots(doctorId, date);
            return Ok(slots);
        }
    }
}
