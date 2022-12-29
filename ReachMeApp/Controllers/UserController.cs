using DomainLayer.Dto;
using DomainLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RepositoryLayer.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReachMeApp.Controllers
{
    public class UserController : Controller
    {
        public Uri baseAddres { get; set; }
        public HttpClient client { get; set; }

        public UserController()
        {
            baseAddres = new Uri("https://localhost:44348/");
            client = new HttpClient();
            client.BaseAddress = baseAddres;
        }

        public IActionResult Index()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Jwt"]);
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "api/Profile").Result;
            var currentUser = JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result);
            
            if(response.IsSuccessStatusCode)
                return View(currentUser);
            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        [Route("/User/Index/{username}")]
        public IActionResult Index(string username)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Jwt"]);
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "api/Profile/" + username).Result;
            var currentUser = JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result);

            if (response.IsSuccessStatusCode)
                return View(currentUser);
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Discover()
        {
            return View();
        }

        public IActionResult Account()
        {
            AccountDto accountDto = new AccountDto
            {
                Email = "",
                Username = "",
                Password = "",
                RepeatPassword = "",
                ProfileBio = "",
                ProfilePic = null
            };
            return View(accountDto);
        }

        [HttpPost]
        public IActionResult Account(AccountDto accountDto)
        {
            AccountDbDto accountDbDto = new AccountDbDto
            {
                Email = accountDto.Email,
                Username = accountDto.Username,
                Password = accountDto.Password,
                ProfileBio = accountDto.ProfileBio,
                ProfilePic = ConvertImage(accountDto.ProfilePic)
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Jwt"]);
            string data = JsonConvert.SerializeObject(accountDbDto);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "api/Account", content).Result;

            if (response.IsSuccessStatusCode)
                return View();
            return RedirectToAction("Index", "User");
        }

        [HttpPost]
        public IActionResult DeleteAccount()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Jwt"]);

            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "api/Account/DeleteAccount", null).Result;

            if (response.IsSuccessStatusCode)
            {
                if (HttpContext.Request.Cookies["Jwt"] != null)
                    Response.Cookies.Delete("Jwt");
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Post()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Jwt"]);
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "api/Post").Result;

            if (response.IsSuccessStatusCode)
            {
                PostDto postDto = new PostDto
                {
                    UserId = 0,
                    Description = "",
                    ImageFile = null
                };
                return View(postDto);
            }
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public IActionResult Post(PostDto postDto)
        {
            PostDbDto postDbDto = new PostDbDto
            {
                UserId = postDto.UserId,
                Description = postDto.Description,
                ImageData = ConvertImage(postDto.ImageFile)
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Jwt"]);
            string data = JsonConvert.SerializeObject(postDbDto);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "api/Post", content).Result;

            if (response.IsSuccessStatusCode)
                return View();
            return RedirectToAction("Index", "User");
        }

        public IActionResult Search(NavigationDto navigationDto)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Jwt"]);
            string data = JsonConvert.SerializeObject(navigationDto.SearchString);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "api/Search", content).Result;
            List<User> searchResults = JsonConvert.DeserializeObject<List<User>>(response.Content.ReadAsStringAsync().Result);

            if (response.IsSuccessStatusCode)
                return View(searchResults);
            return RedirectToAction("Login", "Home");
        }

        private string ConvertImage(IFormFile image)
        {
            byte[] bytes = null;
            if (image != null)
            {
                using (Stream fs = image.OpenReadStream())
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        bytes = br.ReadBytes((Int32)fs.Length);
                        return Convert.ToBase64String(bytes, 0, bytes.Length);
                    }
                }
            }
            return null;
        }
    }
}
