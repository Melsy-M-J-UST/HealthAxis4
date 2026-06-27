using AutoMapper;
using HealthAxis3.API.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis3.API.Controllers
{
    [Route("api/Admin/Report")]
    [ApiController]
    public class AdminAppointmentController(IAppointmentService appointmentService) : ControllerBase
    {
        [HttpGet("")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetReport()
        {
            var result = await appointmentService.GetAppointmentReportAsync();
            return Ok(result);
        }
    }
}
