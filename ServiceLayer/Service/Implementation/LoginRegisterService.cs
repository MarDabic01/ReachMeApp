using DomainLayer.Dto;
using DomainLayer.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Data;
using ServiceLayer.Service.Contract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

            if(user != null)
            {
                var token = GenerateToken(user);
                return "User successfully logged in";
            }
            return "Invalid log in";
        }

        public string Register(RegisterDto newUser)
        {
            User user = new User();

            user.Email = newUser.Email;
            user.Username = newUser.Username;
            user.Password = newUser.Password;

            context.Users.Add(user);
            context.SaveChanges();

            return "User successfully created";
        }
    }
}
