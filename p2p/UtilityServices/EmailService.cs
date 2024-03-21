using MailKit.Net.Smtp;
using MimeKit;
using p2p.Common.Models;


namespace p2p.UtilityServices
{
    public class EmailServices:IEmailServices
    {
        private readonly IConfiguration _config;
        public EmailServices(IConfiguration configuration)
        {
            _config = configuration;
        }
      

        public void SendEmail(EmailModel emailModel)
        {
            var emailMessage = new MimeMessage();
            var from = _config["EmailSettings:From"];
            emailMessage.From.Add(new MailboxAddress("Admin", from));
            emailMessage.To.Add(new MailboxAddress(emailModel.To, emailModel.To));
            emailMessage.Subject = emailModel.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(emailModel.Content)
            };

            using(var client =new SmtpClient())
            {
                try
                {
                    client.Connect(_config["EmailSettings:SmtpServer"],465,true);
                    client.Authenticate(_config["EmailSettings:From"], _config["EmailSettings:Password"]);
                    client.Send(emailMessage);
                }
                catch(Exception ex) {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
