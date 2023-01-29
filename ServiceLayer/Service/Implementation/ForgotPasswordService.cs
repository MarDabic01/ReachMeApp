using DomainLayer.Dto;
using RepositoryLayer.Data;
using ServiceLayer.Service.Contract;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

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
            MailAddress from = new MailAddress("reachme.official15@gmail.com");
            MailMessage message = new MailMessage(from, to);
            message.Subject = "REACH ME - Recover password";
            message.Body = "<html>" +
                "<h1>We are willing to help you</h1>" +
                "<h3>Please recover your password <a href='https://localhost:44355/ForgotPassword/RecoverPassword/" + EncryptString(forgotPassword.Email) + "'>here</a></h3>" +
                "</html>";
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("reachme.official15@gmail.com", "vtrxwkpwceczcven"),
                EnableSsl = true
            };
            client.Send(message);
        }

        public void RecoverPassword(RecoverPasswordDto recoverPassword)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == DecryptString(recoverPassword.EncryptedEmail));

            user.Password = EncryptString(recoverPassword.Password);
            context.SaveChanges();
        }

        public string EncryptString(string word)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(word);
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

        public string DecryptString(string word)
        {
            byte[] data = Convert.FromBase64String(word);
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
