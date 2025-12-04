using AbleEaseCore.Validations;
using AbleEaseDomain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbleEaseCore.DTOs.PatientDTOs
{
    public class UpdatePatient
    {
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces")]
        public string? Name { get; set; }

        [StringLength(250, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 250 characters")]
        public string? Address { get; set; }

        [StringLength(50, ErrorMessage = "Contact info cannot exceed 50 characters")]
        [Phone(ErrorMessage = "Please provide a valid phone number")]
        public string? ContactInfo { get; set; }

        [EnumDataType(typeof(Gender), ErrorMessage = "Invalid gender value")]
        public Gender? Gender { get; set; }

        [DataType(DataType.Date)]
        [BirthDateValidation(ErrorMessage = "Birth date must be valid")]
        public DateTime? BirthDate { get; set; }

        public Guid? RelativeSSN { get; set; }
        public Guid? CaregiverSSN { get; set; }
        public Guid? ProgramOrganizationSSN { get; set; }
        public int? ProgramId { get; set; }
    }
}
