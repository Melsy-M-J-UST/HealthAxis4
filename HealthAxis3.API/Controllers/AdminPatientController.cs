using HealthAxis3.API.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis3.API.Controllers
{
    [Route("api/Admin/Patients")]
    [ApiController]
    public class AdminPatientController(IPatientService patientService) : ControllerBase
    {
        [HttpPut("Deactivate/{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeactivatePatient(int id)
        {
            if (!ModelState.IsValid) return BadRequest();
            var result = await patientService.DeactivatePatientAsync(id);
            if (result == null) return NotFound();
            else return Ok(result);
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme, Roles ="Admin")]
        public async Task<IActionResult> GetAllPatients()
        {
            var result = await patientService.GetAllAsync();
            return Ok(result);
        }
    }
}
