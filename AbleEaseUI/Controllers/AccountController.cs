using AbleEaseCore.DTOs.AccountDTOs;
using AbleEaseCore.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace AbleEaseUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Register")]

        public async Task<IActionResult> Register(RegisterDTO register)
        {
            var message = await _accountService.Register(register);
            return Ok(message);
        }


        [HttpPost("Login")]

        public async Task<IActionResult> Login(LoginDTO login)
        {
            var message = await _accountService.Login(login);

            return Ok(message);



        }





    }
}
