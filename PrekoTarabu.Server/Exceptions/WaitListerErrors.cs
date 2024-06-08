using static PrekoTarabu.Server.Exceptions.Error;

namespace PrekoTarabu.Server.Exceptions;

public class WaitListerErrors
{
    public static readonly Error UserExists = new Error(
        "Waitlister error", "You are already a waitlister");

}