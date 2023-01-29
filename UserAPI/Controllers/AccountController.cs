using DomainLayer.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service.Contract;
using System.Linq;
using System.Security.Claims;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUser userService;
        private readonly ILoginRegister loginRegisterService;
        private readonly IForgotPassword forgotPasswordService;

        public AccountController(IUser userService, ILoginRegister loginRegisterService, IForgotPassword forgotPasswordService)
        {
            this.userService = userService;
            this.loginRegisterService = loginRegisterService;
            this.forgotPasswordService = forgotPasswordService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult ShowAccount()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;
                var currentUser = userService.GetUser(userClaims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value);

                AccountDto accountDto = new AccountDto
                {
                    Id = currentUser.Id,
                    Email = currentUser.Email,
                    Username = currentUser.Username,
                    Password = forgotPasswordService.DecryptString(currentUser.Password),
                    RepeatPassword = forgotPasswordService.DecryptString(currentUser.Password),
                    ProfileBio = currentUser.ProfileBio,
                    ProfilePicData = currentUser.ProfilePic,
                    ProfilePic = null
                };

                return Ok(accountDto);
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize]
        public IActionResult UpdateUser(AccountDto accountDto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;
                var currentUser = userService.GetUser(userClaims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value);
                accountDto.Id = currentUser.Id;

                if (currentUser.Email != accountDto.Email)
                    if (loginRegisterService.IsEmailUsed(accountDto.Email))
                        accountDto.Email = currentUser.Email;
                if (currentUser.Username != accountDto.Username)
                    if (loginRegisterService.IsUsernameUsed(accountDto.Username))
                        accountDto.Username = currentUser.Username;
            }           
            
            userService.UpdateUser(accountDto);
            return Ok();
        }

        [HttpPost("DeleteAccount")]
        [Authorize]
        public IActionResult DeleteAccount()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;
                var currentUser = userService.GetUser(userClaims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value);
                userService.DeleteUser(currentUser.Id);
            }
            return Ok();
        }

    }
}
