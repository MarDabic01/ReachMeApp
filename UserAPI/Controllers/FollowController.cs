using DomainLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service.Contract;
using ServiceLayer.Service.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private readonly IFollow followService;
        private readonly IUser userService;

        public FollowController(IFollow followService, IUser userService)
        {
            this.followService = followService;
            this.userService = userService;
        }

        [Authorize]
        [Route("/api/Followers/{username}")]
        [HttpGet]
        public IActionResult GetFollowers(string username)
        {
            return Ok(followService.GetFollowers(username));
        }

        [Authorize]
        [Route("/api/Followings/{username}")]
        [HttpGet]
        public IActionResult GetFollowings(string username)
        {
            return Ok(followService.GetFollowings(username));
        }

        [Authorize]
        [Route("/api/Suggestions")]
        [HttpGet]
        public IActionResult GetSuggestions()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            User currentUser = new User();

            if (identity != null)
            {
                var userClaims = identity.Claims;
                currentUser = userService.GetUser(userClaims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value);
            }

            return Ok(followService.GetSuggestions(currentUser.Username));
        }
    }
}
