using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;

namespace Szlaki.Shared.MailService
{
    public class MailService : IMailService
    {
        public IConfiguration _configuration { get; }
        
        const string fromPassword = "fromPassword";
        const string subject = "Subject";
        const string body = "Body";
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Send(string subject, string body, object bodyParams, string[] recipients)
        {
            var fromAddress = new MailAddress("kasprzyksmtptest@gmail.com");
            var resultBody = "";
            foreach (var prop in bodyParams.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                resultBody = body.Replace($"{{{prop.Name}}}", prop.GetValue(bodyParams, null).ToString());
            }

            var mailMessage = new MailMessage { IsBodyHtml = true };     

            mailMessage.From = new MailAddress("kasprzyksmtptest@gmail.com");
            mailMessage.Subject = subject;
            mailMessage.Body = resultBody;

            foreach (var recipient in recipients)
            {
                mailMessage.To.Add(recipient);
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,             
                Credentials = new NetworkCredential(fromAddress.Address, "SMTPtest2020")
            };        
            smtp.Send(mailMessage);
        }
    }
}
