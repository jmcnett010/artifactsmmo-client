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
        while (true)
        {
            var commands = new Commands(client);
            await commands.Authenticate(username, password); // Ideally we would only authenticate when our token expires, but the API does not return a TTL

            // TODO - Dont move to the chicken coop if we are already there
            var chickenCoop = new Coordinates(0, 1);
            await commands.Move(character, chickenCoop);

            var result = await commands.Attack(character);

            var combatResult = result.Data;
            Console.WriteLine($"FightResult: {combatResult.Fight.Result}");
            Console.WriteLine($"RemainingSeconds: {combatResult.Cooldown.RemainingSeconds}");

            // Wait until the cooldown is up
            Thread.Sleep(TimeSpan.FromSeconds(combatResult.Cooldown.RemainingSeconds + 1));
        }
    }
}