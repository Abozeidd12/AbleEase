using AbleEaseDomain.Enums;
using AbleEaseCore.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbleEaseCore.DTOs.PatientDTOs
{
    public class AddPatient
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 250 characters")]

        public string? Address { get; set; }

        [Required(ErrorMessage = "Contact information is required")]
        [StringLength(50, ErrorMessage = "Contact info cannot exceed 50 characters")]
        [Phone(ErrorMessage = "Please provide a valid phone number")]
        public string? ContactInfo { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [EnumDataType(typeof(Gender), ErrorMessage = "Invalid gender value")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Birth date is required")]
        [DataType(DataType.Date)]
        [BirthDateValidation(ErrorMessage = "Birth date must be valid")]
        public DateTime BirthDate { get; set; }

        // === Foreign Keys for Relationships (Nullable Guids/int) ===
        public Guid? RelativeSSN { get; set; }
    }
}
