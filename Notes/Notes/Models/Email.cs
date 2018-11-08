using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Notes.Models
{

    class Email
    {
        public static async Task SendMessageAsync(string FromMailAddress, string FromName, string FromPass, string ToMailAddress,
            string ToName, string Subject, string BodyMessage)
        {
            MailAddress fromMailAddress = new MailAddress(FromMailAddress, FromName);
            MailAddress toAddress = new MailAddress(ToMailAddress, ToName);

            using (MailMessage mailMessage = new MailMessage(fromMailAddress, toAddress))
            using (SmtpClient smtpClient = new SmtpClient())
            {
                mailMessage.Subject = Subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = BodyMessage;

                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(fromMailAddress.Address, FromPass);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}