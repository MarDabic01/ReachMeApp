using DomainLayer.Dto;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service.Contract;
using System;

namespace LoginRegisterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginRegisterController : ControllerBase
    {
        private readonly ILoginRegister loginRegister;

        public LoginRegisterController(ILoginRegister loginRegister)
        {
            this.loginRegister = loginRegister;
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginDto loginUser)
        {
            var user = loginRegister.Login(loginUser);

            if (user != null)
                return Ok(user);
            return BadRequest("User not found");
        }

        [HttpPost("Register")]
        public IActionResult Register(RegisterDto newUser)
        {
            if(!loginRegister.IsInfoUsed(newUser.Email, newUser.Username))
            {
                var id = loginRegister.Register(newUser);
                try
                {
                    loginRegister.SendVerificationEmail(newUser.Email, id, "windows");
                }catch(Exception e)
                {
                    loginRegister.SendVerificationEmail(newUser.Email, id, "mac");
                }

                return Ok(id);
            }
            return BadRequest("Email or username already exist");
        }

        [HttpPost("VerifyEmail")]
        public IActionResult VerifyEmail([FromBody]string verify)
        {
            loginRegister.VerifyUser(verify);
            return Ok();
        }
    }
}
