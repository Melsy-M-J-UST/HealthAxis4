using System.ComponentModel.DataAnnotations;

namespace HealthAxis3.API.Models.Dtos.DoctorDto
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Doctor Name is required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 to 30 characters")]
        [RegularExpression(@"^[A-Za-z\s\.]+$", ErrorMessage = "Only alphabets, space and dot allowed")]
        public required string DoctorName { get; set; }

        [Required(ErrorMessage = "Specialisation is required")]
        [RegularExpression(@"^(Cardiologist|Dermatologist|Neurologist|Pediatrician|GeneralPractitioner|Endocrinologist|Gynecologist|Oncologist|OrthopedicSurgeon|Psychiatrist )$", ErrorMessage = "Invalid Specialisation")]
        public required string Specialisation { get; set; }

        [Required(ErrorMessage = "Experience is required")]
        [Range(0, 50, ErrorMessage = "Experience must be between less than 50 years")]

        public int Experience { get; set; }

        [Required(ErrorMessage = "Fees is required")]
        [Range(0, 5000, ErrorMessage = "Fees must a positive number less than 5000")]
        public int Fees { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }
}
