namespace ArtifactsMMO.Client.Models;

/// <summary>
/// This class combines multiple commands to create repeatable actions
/// </summary>
public class Inator(string username, string password)
{
    private readonly HttpClient client = new HttpClient();
    private readonly string username = username;
    private readonly string password = password;


    public async Task FightChickens(string character)
    {
        var commands = new Commands(client);
        await commands.Authenticate(username, password);

        var chickenCoop = new Coordinates(0, 1);

        await commands.Move(character, chickenCoop);
        await commands.Attack(character);
    }
}