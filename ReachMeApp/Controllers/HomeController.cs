using DomainLayer.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

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

        public IActionResult Login()
        {
            TempData.Remove("InvalidLogin");
            return View();
        }

        [HttpGet("Login")]
        public IActionResult Login(int x)
        {
            TempData.Remove("InvalidLogin");
            return View();
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginDto loginUser)
        {
            StringContent content = Serialize<LoginDto>(loginUser);
            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "api/LoginRegister/Login", content).Result;

            string token = response.Content.ReadAsStringAsync().Result;
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddMinutes(15);
            Response.Cookies.Append("Jwt", token, cookieOptions);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index","User");
            TempData["InvalidLogin"] = "User do not exist or is not verified";
            return View();
        }

        public IActionResult Register()
        {
            TempData.Remove("InvalidRegister");
            return View();
        }

        [HttpPost("Register")]
        public IActionResult Register(RegisterDto newUser)
        {
            StringContent content = Serialize<RegisterDto>(newUser);
            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "api/LoginRegister/Register", content).Result;

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            TempData["InvalidRegister"] = "Email or username already in use";
            return View();
        }

        [HttpGet]
        [Route("Home/VerifyEmail/{id}")]
        public IActionResult VerifyEmail(string id)
        {
            VerifyDto dto = new VerifyDto
            {
                UserId = id
            };
            StringContent content = Serialize<string>(id);
            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "api/LoginRegister/VerifyEmail", content).Result;

            if(response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            return BadRequest("Something went wrong, verification failed");
        }

        private StringContent Serialize<T>(T obj)
        {
            string data = JsonConvert.SerializeObject(obj);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            return content;
        }
    }
}
