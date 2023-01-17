using DomainLayer.Dto;
using DomainLayer.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceLayer.Service.Contract;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ReachMeApp.Controllers
{
    public class NavigationController : Controller
    {
        private readonly IUser userService;

        public Uri baseAddres { get; set; }
        public HttpClient client { get; set; }

        public NavigationController(IUser userService)
        {
            baseAddres = new Uri("https://localhost:44348/");
            client = new HttpClient();
            client.BaseAddress = baseAddres;
            this.userService = userService;
        }

        public IActionResult Post()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Jwt"]);
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "api/Post").Result;

            if (response.IsSuccessStatusCode)
                return View();
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public IActionResult Post(PostDto postDto)
        {
            postDto.ImageData = userService.ConvertImage(postDto.ImageFile);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Jwt"]);
            string data = JsonConvert.SerializeObject(postDto);
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
