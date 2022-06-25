using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace SitecoreCaseStudy.Utilities
{
    public class MailSender
    {
        private string randomOTP;

        private string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

        public MailSender()
        {
            this.randomOTP = GenerateRandomOTP(6, saAllowedCharacters);
        }

        public string getOTP()
        {
            return this.randomOTP;
        }

        public void SendOTP(string subject, string emailTo)
        {
            var senderEmail = new MailAddress("buildingbell@outlook.com.vn");
            var receiverEmail = new MailAddress(emailTo);
            var password = "MOYmeoHONG321@";
            var smtp = new SmtpClient
            {
                Host = "smtp-mail.outlook.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var message = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = subject,
                Body = this.randomOTP
            })
            {
                smtp.Send(message);
            }
        }
       
        private string GenerateRandomOTP(int OTPLength, string[] saAllowedCharacters)
        {

            string OTP = String.Empty;
            string tempChars = String.Empty;
            Random rand = new Random();

            for (int i = 0; i < OTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                tempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                OTP += tempChars;
            }
            return OTP;
        }
    }
}