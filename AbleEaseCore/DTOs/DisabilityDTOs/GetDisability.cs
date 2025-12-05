using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbleEaseCore.DTOs.DisabilityDTOs
{
    public class GetDisability
    {

        public Guid SSN { get; set; }

        public string? name { get; set; }
        public string? type { get; set; }
        public string? description { get; set; }
    }



}
