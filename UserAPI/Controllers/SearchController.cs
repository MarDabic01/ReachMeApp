using DomainLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service.Contract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IUser userService;

        public SearchController(IUser userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<List<User>>> Search([FromBody]string searchString)
        {
            return Ok(await userService.SearchResult(searchString));
        }
    }
}
