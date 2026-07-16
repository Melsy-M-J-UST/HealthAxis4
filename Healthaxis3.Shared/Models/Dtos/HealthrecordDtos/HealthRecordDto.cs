using HealthAxis3.Shared.Models.Dtos.AppointmentDtos;
using HealthAxis3.Shared.Models.Dtos.DoctorDtos;
using HealthAxis3.Shared.Models.Dtos.PatientDtos;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxis3.Shared.Models.Dtos.HealthrecordDtos
{
    [ExcludeFromCodeCoverage]
    public class HealthRecordDto
    {
        public int HealthRecordId { get; set; }

        [Required(ErrorMessage = "AppointmentId is required")]
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "PatientId is required")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "DoctorId is required")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Visit date is required")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(HealthRecordDto), nameof(ValidateVisitDate))]
        public DateTime VisitDate { get; set; }

        [Required(ErrorMessage = "Diagnosis is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Diagnosis must be between 3 and 50 characters")]
        public required string Diagnosis { get; set; }

        [Required(ErrorMessage = "Prescription is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Prescription must be between 3 and 100 characters")]
        public required string Prescription { get; set; }

        [StringLength(100, ErrorMessage = "Notes cannot exceed 100 characters")]
        public string Notes { get; set; } = string.Empty;

        [ForeignKey("AppointmentId")]
        public virtual AppointmentDto? Appointment { get; set; }

        [ForeignKey("PatientId")]
        public virtual PatientDto? Patient { get; set; }

        [ForeignKey("DoctorId")]
        public virtual DoctorDto? Doctor { get; set; }

        public static ValidationResult? ValidateVisitDate(DateTime date)
        {
            if (date > DateTime.Today)
            {
                return new ValidationResult("Visit date cannot be in the future");
            }
            return ValidationResult.Success;
        }
    }
}
