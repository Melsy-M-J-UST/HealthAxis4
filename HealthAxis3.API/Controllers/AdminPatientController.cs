using HealthAxis3.API.Service;
using HealthAxis3.API.Service.Implementation;
using HealthAxis3.Shared.Models.Dtos.PatientDtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis3.API.Controllers
{
    [Route("api/Admin/Patients")]
    [ApiController]
    public class AdminPatientController(IPatientService patientService) : ControllerBase
    {
        [HttpPut("{id}/update")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdatePatientStatus(int id, [FromBody] PatientUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var result = await patientService.UpdateStatusAsync(id, dto.IsActive);
            if (result == null) return NotFound();
            else return Ok(result);
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAllPatients()
        {
            var result = await patientService.GetAllAsync();
            return Ok(result);
        }
        [HttpPut("{id}/status")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var result = await patientService.DeactivatePatientAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}
