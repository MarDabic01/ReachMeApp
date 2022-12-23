using DomainLayer.Dto;
using DomainLayer.Model;
using RepositoryLayer.Data;
using ServiceLayer.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service.Implementation
{
    public class ForgotPasswordService : IForgotPassword
    {
        private readonly DataContext context;

        public ForgotPasswordService(DataContext context)
        {
            this.context = context;
        }

        public void SendRecoverEmail(ForgotPasswordDto forgotPassword)
        {
            MailAddress to = new MailAddress(forgotPassword.Email);
            MailAddress from = new MailAddress("reachme915@gmail.com");
            MailMessage message = new MailMessage(from, to);
            message.Subject = "REACH ME - Recover password";
            message.Body = "<html>" +
                "<h1>We are willing to help you</h1>" +
                "<h3>Please recover your password <a href='https://localhost:44355/ForgotPassword/RecoverPassword/" + EncryptEmail(forgotPassword.Email) + "'>here</a></h3>" +
                "</html>";
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("reachme915@gmail.com", "yeljtbfzakhgwaow"),
                EnableSsl = true
            };
            client.Send(message);
        }

        public void RecoverPassword(RecoverPasswordDto recoverPassword)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == DecryptEmail(recoverPassword.EncryptedEmail));

            user.Password = recoverPassword.Password;
            context.SaveChanges();
        }

        private string EncryptEmail(string email)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(email);
            byte[] result;
            using(MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes("whpzjzme"));
                using(TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7})
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    result = transform.TransformFinalBlock(data, 0 , data.Length);
                }
            }
            return Convert.ToBase64String(result, 0, result.Length);
        }

        private string DecryptEmail(string email)
        {
            byte[] data = Convert.FromBase64String(email);
            byte[] result;
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes("whpzjzme"));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    result = transform.TransformFinalBlock(data, 0, data.Length);
                }
            }
            return UTF8Encoding.UTF8.GetString(result);
        }
    }
}
