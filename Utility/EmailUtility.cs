using System.Net.Mail;
using System.Net;

namespace Utility

{
    public class EmailUtility
    {
        public static void  SendEmailViaGmailSmtp(MailMessage mailMessage, string from, string password)
        {
            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.Credentials = new NetworkCredential(from, password);
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
            }
        }
    }
}
