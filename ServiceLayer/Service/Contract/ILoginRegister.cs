using DomainLayer.Dto;
using DomainLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service.Contract
{
    public interface ILoginRegister
    {
        string Register(RegisterDto newUser);
        string Login(LoginDto user);
        string GenerateToken(User user);
        User Authenticate(LoginDto user);
    }
}
