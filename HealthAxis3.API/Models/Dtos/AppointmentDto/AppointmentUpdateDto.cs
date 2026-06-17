using System.ComponentModel.DataAnnotations;

namespace HealthAxis3.API.Models.Dtos.AppointmentDto
{
    public class AppointmentUpdateDto
    {
        public int AppointmentId { get; set; }
        [Required(ErrorMessage = "Status is required")]
        [RegularExpression(@"^(Pending|Cancelled|Confirmed|Completed)$",
            ErrorMessage = "Invalid status")]
        public string Status { get; set; } = "Pending";
    }
}
