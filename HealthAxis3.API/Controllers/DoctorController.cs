using AutoMapper;
using HealthAxis3.API.Service;
using HealthAxis3.API.Service.Implementation;
using HealthAxis3.Shared.Models.Dtos.DoctorDtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController(IDoctorService service, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Patient")]
        public async Task<IActionResult> GetAll()
        {
            var result = await service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("search")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Patient")]
        public async Task<IActionResult> Search([FromQuery] int? id, [FromQuery] string? name, [FromQuery] string? specialisation)
        {
            List<DoctorDto> result = [];
            if (id.HasValue)
            {
                var data = await service.GetByIdAsync(id.Value);
                if (data != null)
                {
                    result.Add(data);
                }
            }
            else if (!string.IsNullOrWhiteSpace(name))
            {
                result = await service.GetByNameAsync(name);
            }
            else if (!string.IsNullOrWhiteSpace(specialisation))
            {
                result = await service.GetBySpecialisationAsync(specialisation);
            }
            else
            {
                return BadRequest("Please provide at least one search parameter.");
            }
            if (result == null || result.Count == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet("doctors/{doctorId}/availability")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Patient")]
        public async Task<IActionResult> GetDoctorAvailability(int doctorId, DateTime date)
        {
            var slots = await service.GetAvailableSlots(doctorId, date);
            return Ok(slots);
        }
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Doctor")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] DoctorDto entity)
        {
            if (!ModelState.IsValid) return BadRequest();
            var savedEntity = mapper.Map<DoctorUpdateDto>(entity);
            var result = await service.UpdateAsync(id, savedEntity);
            if (result == null) return NotFound();
            else return Ok(result);
        }
    }
}
