using AbleEaseCore.DTOs.AccountDTOs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbleEaseCore.IServices
{
    public interface IAccountService
    {
        Task<string> Register(RegisterDTO register);

        Task<string> Login(LoginDTO loginDto);


    }
}
