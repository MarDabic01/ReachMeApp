using DomainLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RepositoryLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
            return View();
        }
    }
}
