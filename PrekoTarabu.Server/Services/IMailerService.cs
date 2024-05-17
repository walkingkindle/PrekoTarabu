using PrekoTarabu.Server.Models;

namespace PrekoTarabu.Server.Services;

public interface IMailerService
{
    void SendEmail(MailerMessage message);

}