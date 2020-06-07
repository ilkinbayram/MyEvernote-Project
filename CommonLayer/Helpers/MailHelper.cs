using MyEvernote.BussinesLayer.Tools;
using MyEvernote.CommonLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Helpers
{
    public class MailHelper
    {
        public static bool SendMail(string body, string to, string subject)
        {
            return SendMail(body,new List<string> { to},subject);
        }
        public static bool SendMail(string body, List<string> to, string subject, bool isHMTL = true)
        {
            bool result = false;
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(ConfigHelper.Get<string>("MailUser"));

                to.ForEach(x =>
                {
                    message.To.Add(new MailAddress(x));
                });
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isHMTL;

                using (var smtp = new SmtpClient(
                    ConfigHelper.Get<string>("MailHost"),
                    ConfigHelper.Get<int>("MailPort")))
                {
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(ConfigHelper.Get<string>("MailUser"), ConfigHelper.Get<string>("MailPass"));
                    smtp.Send(message);
                    result = true;
                };
            }
            catch (Exception)
            {
                
            }


            return result;
        }

        public static string MailSubjectStatus(SubjectStatus status)
        {
            string result = null;
            switch (status)
            {
                case SubjectStatus.AccountConfirmationMail:
                    result = "Account Confirmation By Email";
                    break;
                case SubjectStatus.ChangePasswordRequest:
                    result = "Change Your Password By Email";
                    break;
                case SubjectStatus.DeleteAccountRequest:
                    result = "Do You Want To Delete Your Account Permanently ?";
                    break;
                case SubjectStatus.InformAboutDeletedAccount:
                    result = "Your Account Is Deleted By Administration";
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
