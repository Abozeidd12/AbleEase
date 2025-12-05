using System.ComponentModel.DataAnnotations;

namespace AbleEaseCore.DTOs.DisabilityDTOs
{
    public class UpdateDisability
    {


        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]

        public string? name { get; set; }

        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string? type { get; set; }

        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string? description { get; set; }
    }



}
