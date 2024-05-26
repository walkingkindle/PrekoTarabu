using MimeKit;

namespace PrekoTarabu.Server.Models;

public class MailerMessage
{
    public string From { get; set; }
    public string To { get; set; }
    public static string Name { get; set; }
    public string NameFrom { get; set; }
    public string Subject { get; set; }
    public string? Message { get; set; }
}