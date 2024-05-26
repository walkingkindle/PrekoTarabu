namespace PrekoTarabu.Server.Exceptions;

public class MailerErrors
{
    public static readonly Error LoginError = new Error(
        "Login.Error", "There was an error while logging in");
}