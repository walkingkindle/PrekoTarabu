using System.Linq.Expressions;
using System.Net.Mail;
using MailKit;
using MailKit.Net.Pop3;
using MimeKit;
using PrekoTarabu.Server.Controllers;
using PrekoTarabu.Server.Credentials;
using PrekoTarabu.Server.Exceptions;
using PrekoTarabu.Server.Models;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace PrekoTarabu.Server.Services;

public class MailerService(IConfiguration configuration)
{
    MailSender _mailSender = new MailSender(configuration: configuration);

    //when someone joins the waitlist, An email is sent to the admin @his personal mail
    //save from to the database and his name in Waitlist.dbo
    //Then send him a reaching out email that we received his contribution
    public Result SendEmail(MailerMessage mailerMessage)
    {

        
        using (var client = new SmtpClient())
        {
            Console.WriteLine(_mailSender.AppPassword,_mailSender.Email,_mailSender.SmtpPort);
            client.Connect(_mailSender.SmtpServer,_mailSender.SmtpPort,false);
            client.Authenticate(_mailSender.Email, _mailSender.AppPassword);
            if (client.IsAuthenticated)
            {
                client.Send(MakeMessage(mailerMessage));
                client.Disconnect(true);
            }
            else
            {
                return Result.Failure(MailerErrors.LoginError);
            }

            return Result.Success();

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
    public Result NotifyAdmin(WaitListRequester requester)
    {
            MailerMessage message = new MailerMessage()
            {
                From = _mailSender.Email,
                To = _mailSender.MailAdmin,
                Message = $"{requester.Message ?? "No message provided"}," +
                          $" {requester.Email}",
                Subject =
                    "Looks like someone just joined your WaitList!!",
                NameFrom = requester.Name
            };
            SendEmail(message);

           return Result.Success();

    }

    public Result ReachOut(WaitListRequester requester)
    {
        MailerMessage message = new MailerMessage()
        {
            From = _mailSender.Email,
            To = requester.Email,
            Message = $"Hey thanks for subscribing I'll let you know when it gets here brooooooooooooooo",
            NameFrom = requester.Name,
            Subject = "Reaching out"
        };
        SendEmail(message);

        return Result.Success();
    }

    
}
