using System.Net.Mail;
using System.Net;

namespace Utility;

/// <summary>
/// Provides utility methods for sending emails.
/// </summary>
public static class EmailUtility
{
    /// <summary>
    /// Sends an email message using Gmail's SMTP server with the specified sender credentials.
    /// </summary>
    /// <param name="mailMessage"></param>
    /// <param name="from"></param>
    /// <param name="password"></param>
    public static void  SendEmailViaGmailSmtp(MailMessage mailMessage, string from, string password)
    {
        using var smtpClient = new SmtpClient("smtp.gmail.com", 587);
        smtpClient.Credentials = new NetworkCredential(from, password);
        smtpClient.EnableSsl = true;
        smtpClient.Send(mailMessage);
    }
}
