using DomainLayer.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return Ok(loginRegister.Register(newUser));
        }
    }
}
