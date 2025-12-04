using AbleEaseInfrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbleEaseCore.IServices
{
     public interface ITokenService
    {

        Task<string> GenerateToken(ApplicationUser applicationUser);
    }
}
