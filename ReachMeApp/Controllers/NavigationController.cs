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
    public class NavigationController : Controller
    {
        public Uri baseAddres { get; set; }
        public HttpClient client { get; set; }

        public NavigationController()
        {
            baseAddres = new Uri("https://localhost:44348/");
            client = new HttpClient();
            client.BaseAddress = baseAddres;
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
                //ImageData = ConvertImage(postDto.ImageFile)
                ImageData = ""
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
    }
}
