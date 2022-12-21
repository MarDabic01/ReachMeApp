﻿using DomainLayer.Dto;
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
            if(!loginRegister.IsInfoUsed(newUser))
            {
                var id = loginRegister.Register(newUser);
                loginRegister.SendVerificationEmail(newUser.Email, id);

                return Ok(id);
            }
            return BadRequest("Email or username already exist");
        }

        [HttpPost("VerifyEmail")]
        public IActionResult VerifyEmail(VerifyDto verify)
        {
            loginRegister.VerifyUser(verify.UserId);
            return Ok();
        }
    }
}
