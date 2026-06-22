using AutoMapper;
using HealthAxis3.Shared.Models.Dtos.DoctorDtos;
using HealthAxis3.API.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis3.API.Controllers
{
    [Route("api/Admin/Doctors")]
    [ApiController]
    public class AdminDoctorController(IDoctorService doctorService,IMapper mapper) : ControllerBase
    {
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] DoctorDto entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var result = await doctorService.AddAsync(entity);
                if (result == null) return NotFound();
                else return CreatedAtAction("GetById", new { result.DoctorId }, result);
            }
        }
        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] DoctorDto entity)
        {
            if (!ModelState.IsValid) return BadRequest();
            var savedEntity = mapper.Map<DoctorUpdateDto>(entity);
            var result = await doctorService.UpdateAsync(id, savedEntity);
            if (result == null) return NotFound();
            else return Ok(result);
        }        
        [HttpPut("DoctorDeactivate/{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeactivateDoctor(int id)
        {
            if (!ModelState.IsValid) return BadRequest();
            var result = await doctorService.DeactivateDoctorAsync(id);
            if (result == null) return NotFound();
            else return Ok(result);
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var result = await doctorService.GetAllAsync();
            return Ok(result);
        }
    }
}
