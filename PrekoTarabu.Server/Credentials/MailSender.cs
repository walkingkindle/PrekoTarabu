
namespace PrekoTarabu.Server.Credentials
{
    public class MailSender
    {
        public string SmtpServer { get; private set; }
        public int SmtpPort { get; private set; }
        public string Email { get; private set; }
        public string AppPassword { get; private set; }

        public MailSender(IConfiguration configuration)
        {
            SmtpServer = configuration["SMTP_SERVER"];
            SmtpPort = int.Parse(configuration["SMTP_PORT"]);
            Email = configuration["MAIL_MAIL"];
            AppPassword = configuration["MAIL_PASSWORD"];
        }
    }
}