using DomainLayer.Dto;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service.Contract;
using System;

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
            try
            {
                forgotPassword.SendRecoverEmail(forgotPasswordDto, "windows");
            }catch(Exception e)
            {
                forgotPassword.SendRecoverEmail(forgotPasswordDto, "mac");
            }
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
