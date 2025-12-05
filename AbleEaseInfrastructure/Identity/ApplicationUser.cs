using AbleEaseDomain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbleEaseInfrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public UserRole UserRole { get; set; }

        public string? Name { get; set; }


              


       
    }
}
