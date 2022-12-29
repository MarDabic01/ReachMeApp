using DomainLayer.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
        public IActionResult UpdateUser(AccountDbDto accountDbDto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;
                var currentUser = userService.GetUser(userClaims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value);
                accountDbDto.Id = currentUser.Id;
            }
            userService.UpdateUser(accountDbDto);
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
