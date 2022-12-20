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

        [HttpPost("login")]
        public IActionResult Login(LoginDto loginUser)
        {
            return Ok(this.loginRegister.Login(loginUser));
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto newUser)
        {
            var response = loginRegister.Register(newUser);
            return Ok(response);
        }
    }
}
