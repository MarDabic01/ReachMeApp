﻿using DomainLayer.Dto;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service.Contract;

namespace LoginRegisterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {
        private readonly IForgotPassword forgotPassword;

        public ForgotPasswordController(IForgotPassword forgotPassword)
        {
            this.forgotPassword = forgotPassword;
        }

        [HttpPost("SendRecoverEmail")]
        public IActionResult SendRecoverEmail(ForgotPasswordDto forgotPasswordDto)
        {
            forgotPassword.SendRecoverEmail(forgotPasswordDto);
            return Ok();
        }

        [HttpPost("RecoverPassword")]
        public IActionResult RecoverPassword(RecoverPasswordDto recoverPasswordDto)
        {
            forgotPassword.RecoverPassword(recoverPasswordDto);
            return Ok();
        }
    }
}
