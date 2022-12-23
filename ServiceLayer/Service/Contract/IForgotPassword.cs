using DomainLayer.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service.Contract
{
    public interface IForgotPassword
    {
        void SendRecoverEmail(ForgotPasswordDto forgotPassword);
        void RecoverPassword(RecoverPasswordDto recoverPassword);
    }
}
