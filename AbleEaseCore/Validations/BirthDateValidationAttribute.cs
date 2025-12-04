using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbleEaseCore.Validations
{
    public class BirthDateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is DateTime birthDate)
            {
                var today = DateTime.Today;

                // Check if date is in the future
                if (birthDate > today)
                {
                    return new ValidationResult("Birth date cannot be in the future");
                }

                // Check minimum date (reasonable lower bound)
                if (birthDate < new DateTime(1900, 1, 1))
                {
                    return new ValidationResult("Birth date cannot be before January 1, 1900");
                }

                // Check if person is too old (e.g., more than 150 years)
                var age = today.Year - birthDate.Year;
                if (birthDate.Date > today.AddYears(-age)) age--;

                if (age > 150)
                {
                    return new ValidationResult("Invalid birth date - age cannot exceed 150 years");
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid birth date format");
        }
    }
}
