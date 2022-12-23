using DomainLayer.Dto;
using DomainLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Data;
using ServiceLayer.Service.Contract;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace ServiceLayer.Service.Implementation
{
    public class LoginRegisterService : ILoginRegister
    {
        private readonly DataContext context;
        private readonly IConfiguration config;

        public LoginRegisterService(DataContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }

        public User Authenticate(LoginDto loginUser)
        {
            var user = context.Users.FirstOrDefault(u => u.Username == loginUser.Username && u.Password == loginUser.Password);

            if (user != null)
                return user;
            return null;
        }

        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("pmqhogxiectidborfdsi"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                "https://localhost:44355/",
                "https://localhost:44355/",
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string Login(LoginDto loginUser)
        {
            var user = Authenticate(loginUser);

            if(user != null && IsUserVerified(user))
                return GenerateToken(user);
            return null;
        }

        public string Register(RegisterDto newUser)
        {
            User user = new User();

            user.Email = newUser.Email;
            user.Username = newUser.Username;
            user.Password = newUser.Password;

            context.Users.Add(user);
            context.SaveChanges();

            return user.Id.ToString();
        }

        public void SendVerificationEmail(string email, string id)
        {
            MailAddress to = new MailAddress(email);
            MailAddress from = new MailAddress("reachme915@gmail.com");
            MailMessage message = new MailMessage(from, to);
            message.Subject = "REACH ME - Verification message";
            message.Body = "<html>" +
                "<h1>Welcome to ReachMe</h1>" +
                "<h3>Please verify your e-mail <a href='https://localhost:44355/Home/VerifyEmail/"+ id +"'>here</a></h3>" +
                "</html>";
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("reachme915@gmail.com", "yeljtbfzakhgwaow"),
                EnableSsl = true
            };
            client.Send(message);
        }

        public void VerifyUser(string id)
        {
            var user = context.Users.FirstOrDefault(u => u.Id.ToString() == id);

            user.IsVerified = true;
            context.SaveChanges();
        }

        public bool IsInfoUsed(RegisterDto user)
        {
            if(context.Users.FirstOrDefault(u => u.Username == user.Username) == null && context.Users.FirstOrDefault(u => u.Email == user.Email) == null)
                return false;
            return true;
        }

        public bool IsUserVerified(User user)
        {
            return context.Users.FirstOrDefault(u => u.Id == user.Id).IsVerified == true ? true : false;
        }
    }
}
