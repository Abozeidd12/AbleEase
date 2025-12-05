using System.ComponentModel.DataAnnotations;

namespace AbleEaseCore.DTOs.DisabilityDTOs
{
    public class AddDisability
    {


        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]

        public string? name { get; set; }

        [Required(ErrorMessage = "Disabilty type is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string? type { get; set; }

        [Required(ErrorMessage = "Disability description is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string? description { get; set; }
    }



}
