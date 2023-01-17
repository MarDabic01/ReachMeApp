using DomainLayer.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace ReachMeApp.Controllers
{
    public class ForgotPasswordController : Controller
    {
        public Uri baseAddres { get; set; }
        public HttpClient client { get; set; }

        public ForgotPasswordController()
        {
            baseAddres = new Uri("https://localhost:44389/");
            client = new HttpClient();
            client.BaseAddress = baseAddres;
        }

        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Index(ForgotPasswordDto forgotPassword)
        {
            string data = JsonConvert.SerializeObject(forgotPassword);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "api/ForgotPassword/SendRecoverEmail", content).Result;

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpGet]
        [Route("ForgotPassword/RecoverPassword/{encryptedEmail}")]
        public IActionResult RecoverPassword() => View();

        [HttpPost]
        [Route("ForgotPassword/RecoverPassword/{encryptedEmail}")]
        public IActionResult RecoverPassword(RecoverPasswordDto recoverPasswordDto, string encryptedEmail)
        {
            recoverPasswordDto.EncryptedEmail = encryptedEmail;

            string data = JsonConvert.SerializeObject(recoverPasswordDto);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "api/ForgotPassword/RecoverPassword", content).Result;

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Login", "Home");
            return View();
        }
    }
}
