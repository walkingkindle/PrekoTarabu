using System.Net.Mail;
using MailKit;
using MailKit.Net.Pop3;
using MimeKit;
using PrekoTarabu.Server.Controllers;
using PrekoTarabu.Server.Credentials;
using PrekoTarabu.Server.Models;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace PrekoTarabu.Server.Services;

public class MailerService:IMailerService
{
    private readonly IConfiguration _configuration;

    public MailerService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
 
    //when someone joins the waitlist, An email is sent to the admin @his personal mail
    //save from to the database and his name in Waitlist.dbo
    //Then send him a reaching out email that we received his contribution
    public void SendEmail(MailerMessage mailerMessage)
    {
        MailSender mailSender = new MailSender(configuration: _configuration);

        
        using (var client = new SmtpClient())
        {
            client.Connect(mailSender.SmtpServer,mailSender.SmtpPort,false);
            client.Authenticate(mailSender.Email, mailSender.AppPassword);
            if (client.IsAuthenticated)
            {
                client.Send(MakeMessage(mailerMessage));
                client.Disconnect(true);
            }

        }
    }

    private MimeMessage MakeMessage(MailerMessage mailerMessage)
    {
        var message = new MimeMessage(); 
        message.From.Add(new MailboxAddress(MailerMessage.Name,mailerMessage.From)); 
        message.To.Add(new MailboxAddress(mailerMessage.To,mailerMessage.To));
        message.Subject = mailerMessage.Subject;
        message.Body = new TextPart("plain")
        {
            Text = mailerMessage.Message
        };
        return message;

    }
    
    //send message from Main email to admin email
    public bool NotifyAdmin(WaitListRequester requester)
    {
        MailerMessage message = new MailerMessage(){From = _configuration["MAIL_MAIL"],To = _configuration["MAIL_ADMIN"],
            Message = $"{requester.Message}," +
                      $" {requester.Email}",Subject =
                "Looks like someone just joined your WaitList!!",NameFrom = requester.Name};
        SendEmail(message);
        
        return true;

    }

    public bool ReachOut(WaitListRequester requester)
    {
        MailerMessage message = new MailerMessage()
        {
            From = _configuration["MAIL_MAIL"], To = requester.Email,
            Message = $"Hey thanks for subscribing I'll let you know when it gets here brooooooooooooooo",
            NameFrom = requester.Name,
            Subject = "Reaching out"
        };
        SendEmail(message);
        
        return true;
    }

    
}
