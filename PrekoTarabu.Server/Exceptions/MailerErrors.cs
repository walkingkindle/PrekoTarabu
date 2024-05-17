namespace PrekoTarabu.Server.Exceptions;

public class MailerErrors
{
    public static readonly Error ConnectionError = new Error("Connection/WrongPasswordError"
        , "It looks like there has been a connection error");
    
    
}