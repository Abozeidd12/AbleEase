using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbleEaseCore.DTOs.DisabilityDTOs
{
    public class PateintDisabilityDto
    {

        public class PatientDisabilityDto
        {


            [Required(ErrorMessage = "PatientSSN is required")]

            public Guid PatientSSN { get; set; }

            [Required(ErrorMessage = "DisabilotyID is required")]

            public Guid DisabilityID { get; set; }
            public string? DisabilityName { get; set; }

            [Required(ErrorMessage = "Level of severity is required")]

            public string? Level { get; set; }
            public string? Notes { get; set; }
        }



    }
}
