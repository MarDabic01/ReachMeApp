using DomainLayer.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ReachMeApp.Controllers
{
    public class HomeController : Controller
    {
        public Uri baseAddres { get; set; }
        public HttpClient client { get; set; }

        public HomeController()
        {
            baseAddres = new Uri("https://localhost:44389/");
            client = new HttpClient();
            client.BaseAddress = baseAddres;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Showing Login view
        public IActionResult Login()
        {
            return View();
        }
        //Handling Login post method
        [HttpPost("Login")]
        public IActionResult Login(LoginDto loginUser)
        {
            //Serializing loginUser and consuming LoginRegister API
            string data = JsonConvert.SerializeObject(loginUser);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "api/LoginRegister/Login", content).Result;

            string token = response.Content.ReadAsStringAsync().Result;

            //Storing jwt token into cookie local storage
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddMinutes(15);
            Response.Cookies.Append("Jwt", token, cookieOptions);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            return View();
        }

        //Showing Register view
        public IActionResult Register()
        {
            return View();
        }
        //Handling Register post method
        [HttpPost("Register")]
        public IActionResult Register(RegisterDto newUser)
        {
            //Serializing loginUser and consuming LoginRegister API
            string data = JsonConvert.SerializeObject(newUser);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "api/LoginRegister/Register", content).Result;

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            return View();
        }
    }
}
