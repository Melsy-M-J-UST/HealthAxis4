using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HealthAxis3.Shared.Models.Dtos.PatientDtos
{
    public class PatientDto
    {
        [Required(ErrorMessage = "PatientId is required")]
        public int PatientId { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only alphabets and space is allowed")]
        public string PatientName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        //[CustomValidation(typeof(Patient), nameof(ValidateDOB))]
        [JsonIgnore]
        public DateTime DateOfBirth { get; set; }

        [NotMapped]
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                int age = today.Year - DateOfBirth.Year;

                if (DateOfBirth.Date > today.AddYears(-age))
                {
                    age--;
                }

                return age;
            }
        }

        [Required]
        [RegularExpression(@"^(Male|Female|Transgender|Others)$",
                    ErrorMessage = "Gender must be Male, Female, Transgender or Others")]
        public string Gender { get; set; }

        [Required]
        [RegularExpression(@"^[6-9][0-9]{9}$",
            ErrorMessage = "Enter valid 10 digit phone number")]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [RegularExpression(@"^INS[0-9]{4}$",
                    ErrorMessage = "InsuranceId must be in format INS1234")]
        public string InsuranceId { get; set; }

        public DateTime RegisteredDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        public static ValidationResult ValidateDOB(DateTime dob)
        {
            if (dob > DateTime.Today)
            {

                return new ValidationResult("Date of Birth cannot be in the future");
            }

            return ValidationResult.Success;
        }
    }
}
