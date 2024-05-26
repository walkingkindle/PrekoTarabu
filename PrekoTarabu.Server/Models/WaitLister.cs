namespace PrekoTarabu.Server.Models;

public class WaitLister
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public string? HisHerMessage { get; set; }
    public required string HisHerMail { get; set; }
}