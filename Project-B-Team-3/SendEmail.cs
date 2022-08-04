using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Text.Json;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace ProjectB
{
    class SendEmail
    {
        public static void SendVerifyEmail(string mail, string file, Dictionary<string,string> vars)
        {            
            var emailVerifyBody = System.IO.File.ReadAllText(@$"Data{Path.DirectorySeparatorChar}{file}");
            var stmpClient = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("schoolhbo1@outlook.com", "BeetjeLerenToch"),
                EnableSsl = true
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("schoolhbo1@outlook.com"),
                Subject = "Verify account!",
                Body = ReplaceVars(emailVerifyBody, vars),
                IsBodyHtml = true
            };

            mailMessage.To.Add(mail);
            stmpClient.Send(mailMessage);
        }
        public static void SendReservationEmail(string mail, string file, Dictionary<string, string> vars)
        {
            var emailVerifyBody = System.IO.File.ReadAllText(@$"Data{Path.DirectorySeparatorChar}{file}");
            var stmpClient = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("schoolhbo1@outlook.com", "BeetjeLerenToch"),
                EnableSsl = true
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("schoolhbo1@outlook.com"),
                Subject = "Your Reservation!",
                Body = ReplaceVars(emailVerifyBody, vars),
                IsBodyHtml = true
            };

            mailMessage.To.Add(mail);
            stmpClient.Send(mailMessage);
        }

        public static string ReplaceVars(string file, Dictionary<string,string> vars)
        {
            foreach (KeyValuePair<string, string> entry in vars)
            {
                file = file.Replace(entry.Key,entry.Value.ToString());
            }

            return file;
        }

        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
        }

        public static string Captcha()
        {
            Random rnd = new Random();
            int number = rnd.Next(1000000);
            while (number.ToString().Length != 6)
            {
                number = rnd.Next(1000000);
            }

            return number.ToString();
        }
    }
}
