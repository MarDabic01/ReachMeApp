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

        public AccountController(IUser userService)
        {
            this.userService = userService;
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
