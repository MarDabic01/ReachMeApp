using DomainLayer.Dto;

namespace ServiceLayer.Service.Contract
{
    public interface IForgotPassword
    {
        void SendRecoverEmail(ForgotPasswordDto forgotPassword, string system);
        void RecoverPassword(RecoverPasswordDto recoverPassword);
        string EncryptString(string word);
        string DecryptString(string word);
    }
}
