
namespace PrekoTarabu.Server.Credentials
{
    public class MailSender
    {
        public string SmtpServer { get; private set; }
        public int SmtpPort { get; private set; }
        public string Email { get; private set; }
        public string AppPassword { get; private set; }
        
        public string MailAdmin { get; private set; }

        public MailSender(IConfiguration configuration)
        {
            SmtpServer = configuration["Credentials:SMTP_SERVER"];
            SmtpPort = int.Parse(configuration["Credentials:SMTP_PORT"]);
            Email = configuration["Credentials:MAIL_MAIL"];
            AppPassword = configuration["Credentials:MAIL_PASSWORD"];
            MailAdmin = configuration["Credentials:MAIL_ADMIN"];
        }
    }
}