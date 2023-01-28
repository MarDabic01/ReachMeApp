using DomainLayer.Dto;
using DomainLayer.Model;
using Microsoft.AspNetCore.Http;
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
    public class UserController : Controller
    {
        private readonly IUser userService;

        public Uri baseAddres { get; set; }
        public HttpClient client { get; set; }

        public UserController(IUser userService)
        {
            baseAddres = new Uri("https://localhost:44348/");
            client = new HttpClient();
            client.BaseAddress = baseAddres;
            this.userService = userService;
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
        [Route("/User/Profile/{username}")]
        public IActionResult Profile(string username)
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
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Jwt"]);
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "api/Profile").Result;
            var currentUser = JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result);

            if (response.IsSuccessStatusCode)
                return View(currentUser);
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Discover()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Jwt"]);
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "api/Suggestions").Result;
            var suggestedUsers = JsonConvert.DeserializeObject<List<User>>(response.Content.ReadAsStringAsync().Result);

            if (response.IsSuccessStatusCode)
                return View(suggestedUsers);
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Account()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Jwt"]);
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "api/Account").Result;
            var currentUser = JsonConvert.DeserializeObject<AccountDto>(response.Content.ReadAsStringAsync().Result);

            if (response.IsSuccessStatusCode)
                return View(currentUser);
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public IActionResult Account(AccountDto accountDto)
        {
            accountDto.ProfilePicData = userService.ConvertImage(accountDto.ProfilePic);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Jwt"]);
            string data = JsonConvert.SerializeObject(accountDto);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "api/Account", content).Result;

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "User");
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

        [HttpGet]
        [Route("/User/Followers/{username}")]
        public IActionResult Followers(string username)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Jwt"]);
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "api/Followers/" + username).Result;
            var followers = JsonConvert.DeserializeObject<List<User>>(response.Content.ReadAsStringAsync().Result);

            if (response.IsSuccessStatusCode)
                return View(followers);
            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        [Route("/User/Followings/{username}")]
        public IActionResult Followings(string username)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Jwt"]);
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "api/Followings/" + username).Result;
            var followings = JsonConvert.DeserializeObject<List<User>>(response.Content.ReadAsStringAsync().Result);

            if (response.IsSuccessStatusCode)
                return View(followings);
            return RedirectToAction("Login", "Home");
        }
    }
}
