using DomainLayer.Dto;

namespace ServiceLayer.Service.Contract
{
    public interface IForgotPassword
    {
        void SendRecoverEmail(ForgotPasswordDto forgotPassword);
        void RecoverPassword(RecoverPasswordDto recoverPassword);
    }
}
