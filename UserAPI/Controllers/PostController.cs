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
    public class PostController : ControllerBase
    {
        private readonly IUser userService;

        public PostController(IUser userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult PostPicture()
        {
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public IActionResult PostPicture(PostDto post)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;
                var currentUser = userService.GetUser(userClaims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value);
                post.UserId = currentUser.Id;
            }
            userService.PostPicture(post);
            return Ok();
        }
    }
}
