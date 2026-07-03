using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HealthAxis3.Shared.Models.Dtos.PatientDtos
{
    public class PatientDto
    {
        public int PatientId { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only alphabets and space is allowed")]
        public required string PatientName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(PatientDto), nameof(ValidateDOB))]
        public DateTime DateOfBirth { get; set; }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                int age = today.Year - DateOfBirth.Year;

                if (DateOfBirth > today.AddYears(-age))
                {
                    age--;
                }

                return age;
            }
        }

        [Required]
        [RegularExpression(@"^(Male|Female|Transgender|Others)$",
                    ErrorMessage = "Gender must be Male, Female, Transgender or Others")]
        public required string Gender { get; set; }

        [Required]
        [RegularExpression(@"^[6-9][0-9]{9}$",
            ErrorMessage = "Enter valid 10 digit phone number")]
        public required string PhoneNumber { get; set; }
        [Required]

        [EmailAddress]
        public required string Email { get; set; }

        [RegularExpression(@"^INS[0-9]{4}$",
                    ErrorMessage = "InsuranceId must be in format INS1234")]
        public string InsuranceId { get; set; } = string.Empty;

        public DateTime RegisteredDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        public static ValidationResult? ValidateDOB(DateTime dob)
        {
            if (dob > DateTime.Today)
            {

                return new ValidationResult("Date of Birth cannot be in the future");
            }

            return ValidationResult.Success;
        }
    }
}
