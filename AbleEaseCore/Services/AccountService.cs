using AbleEaseCore.DTOs.AccountDTOs;
using AbleEaseCore.IServices;
using AbleEaseInfrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbleEaseCore.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AccountService> _logger;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService, ILogger<AccountService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<string> Login(LoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return "Wrong Username or Password";

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return "Wrong Email or Password";

            return await _tokenService.GenerateToken(user);



        }

        public async Task<string> Register(RegisterDTO register)
        {
            ApplicationUser user = new ApplicationUser
            {
                Email = register.Email,
                UserName = register.Email,
                Name = register.Name,
                UserRole = register.Role

            };

            var result = await _userManager.CreateAsync(user, register.Password);


            if (result.Succeeded)
                return "Succeded";
            else return $"Registeration failed : {result.Errors.Select(e => e.Description)} ";






        }
    }
}
