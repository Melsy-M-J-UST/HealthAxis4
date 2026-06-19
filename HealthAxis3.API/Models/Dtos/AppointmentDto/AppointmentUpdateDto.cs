using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxis3.API.Models.Dtos.AppointmentDto
{
    [ExcludeFromCodeCoverage]
    public class AppointmentUpdateDto
    {
        public int AppointmentId { get; set; }
        [Required(ErrorMessage = "Status is required")]
        [RegularExpression(@"^(Pending|Cancelled|Confirmed|Completed)$",
            ErrorMessage = "Invalid status")]
        public string Status { get; set; } = "Pending";
    }
}
