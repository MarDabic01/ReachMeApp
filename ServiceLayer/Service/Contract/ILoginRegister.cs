using DomainLayer.Dto;
using DomainLayer.Model;

namespace ServiceLayer.Service.Contract
{
    public interface ILoginRegister
    {
        string Register(RegisterDto newUser);
        string Login(LoginDto user);
        string GenerateToken(User user);
        User Authenticate(LoginDto user);
        void SendVerificationEmail(string email, string token);
        void VerifyUser(string id);
        bool IsInfoUsed(string email, string username);
        bool IsUsernameUsed(string username);
        bool IsEmailUsed(string email);
        bool IsUserVerified(User user);
    }
}
