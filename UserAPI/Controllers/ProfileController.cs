using DomainLayer.Dto;
using DomainLayer.Model;
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
    public class ProfileController : ControllerBase
    {
        private readonly IUser userService;

        public ProfileController(IUser userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult ShowProfile()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if(identity != null)
            {
                var userClaims = identity.Claims;
                return Ok(userService.GetUser(userClaims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value));
            }
            return BadRequest();
        }
    }
}
