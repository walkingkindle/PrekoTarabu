using System.Linq.Expressions;
using System.Net.Mail;
using MailKit;
using MailKit.Net.Pop3;
using MimeKit;
using MimeKit.Text;
using PrekoTarabu.Server.Controllers;
using PrekoTarabu.Server.Credentials;
using PrekoTarabu.Server.Exceptions;
using PrekoTarabu.Server.Models;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace PrekoTarabu.Server.Services;

public class MailerService
{
    private readonly AppDbContext _dbContext;
    private MailSender _mailSender;

    public MailerService(AppDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mailSender = new MailSender(configuration: configuration);
    }

    public void InitializeMailSender(IConfiguration configuration)
    {
        _mailSender = new MailSender(configuration: configuration);
    }
    
    public Result SendEmail(MimeMessage mailerMessage)
    {

        
        using (var client = new SmtpClient())
        {
            client.Connect(_mailSender.SmtpServer,_mailSender.SmtpPort,false);
            client.Authenticate(_mailSender.Email, _mailSender.AppPassword);
            if (client.IsAuthenticated)
            {
                client.Send(mailerMessage);
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

        message.From.Add(new MailboxAddress(mailerMessage.NameFrom, mailerMessage.From)); 
        message.To.Add(new MailboxAddress(mailerMessage.To, mailerMessage.To));
        message.Subject = mailerMessage.Subject;

        // Construct the path to the HTML template file
        string currentDirectory = Directory.GetCurrentDirectory();
        string templatePath = Path.Combine(currentDirectory, "Assets", "mail.html");

        // Ensure the template file exists
        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException("The email template file was not found.", templatePath);
        }

        // Load the HTML template and replace placeholders
        string emailBody = File.ReadAllText(templatePath).Replace("[Name]", mailerMessage.Name);

        // Set the body of the email to HTML format
        message.Body = new TextPart(TextFormat.Html)
        {
            Text = emailBody
        };

        return message;
    }

    
    //send message from Main email to admin email
    public Result NotifyAdmin(WaitListRequester requester)
    {
        MimeMessage message = new MimeMessage();
           message.From.Add(new MailboxAddress("Aleksa",_mailSender.Email));
           message.To.Add(new MailboxAddress("Aleksa", _mailSender.MailAdmin));
           message.Subject = "Looks like someone subscribed to your WaitList";
           message.Body = new TextPart(TextFormat.Plain)
           {
               Text = $" His Mail: {requester.Email}, His Message: {requester.Message ?? "No message"}"
           };
        SendEmail(message);
        if (isWaitlisterAlreadySubscribed(requester.Email))
        {
            return Result.Failure(error: WaitListerErrors.UserExists);
        }
        
        
        
        return Result.Success();

    }

    public Result ReachOut(WaitListRequester requester)
    {
        MailerMessage message = new MailerMessage()
        {
            From = _mailSender.Email,
            To = requester.Email,
            Message = "",
            NameFrom = requester.Name,
            Subject = "Reaching out",
            Name = requester.Name
        };
        SendEmail(MakeMessage(message));
        
        if (isWaitlisterAlreadySubscribed(requester.Email))
        {
            return Result.Failure(error: WaitListerErrors.UserExists);
        }

        return Result.Success();
    }

    private bool isWaitlisterAlreadySubscribed(string waitlisterEmail)
    {
        bool waitListerExists = _dbContext.WaitListers.Any(c => c.HisHerMail == waitlisterEmail);
        return waitListerExists;


    }
    
}
