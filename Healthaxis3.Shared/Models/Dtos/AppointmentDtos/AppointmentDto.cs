using HealthAxis3.Shared.Models.Dtos.DoctorDtos;
using HealthAxis3.Shared.Models.Dtos.PatientDtos;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace HealthAxis3.Shared.Models.Dtos.AppointmentDtos
{
    [ExcludeFromCodeCoverage]
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "PatientId is required")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "DoctorId is required")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Scheduled date is required")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(AppointmentDto), nameof(ValidateAppointmentDate))]
        public DateTime ScheduledDate { get; set; }

        [Required(ErrorMessage = "Slot is required")]
        [RegularExpression(
            @"^(09:00 AM|10:00 AM|11:00 AM|12:00 PM|02:00 PM|03:00 PM|04:00 PM|05:00 PM)$",
            ErrorMessage = "Select a valid time slot")]
        public required string Slot { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression(@"^(Pending|Cancelled|Confirmed|Completed)$",
            ErrorMessage = "Invalid status")]
        public string Status { get; set; } = "Pending";
        [StringLength(200, ErrorMessage = "Cancellation reason cannot exceed 200 characters")]
        public string CancellationReason { get; set; } = string.Empty;

        [ForeignKey("PatientId")]
        public required virtual PatientDto Patient { get; set; }

        [ForeignKey("DoctorId")]
        public required virtual DoctorDto Doctor { get; set; }

        public static ValidationResult? ValidateAppointmentDate(DateTime date)
        {
            DateTime today = DateTime.Today;
            DateTime maxDate = today.AddMonths(6);

            if (date <= today)
                return new ValidationResult("Appointment must be booked from tomorrow onwards");

            if (date > maxDate)
                return new ValidationResult("Appointment cannot be booked beyond 6 months");

            return ValidationResult.Success;
        }
    }
}
