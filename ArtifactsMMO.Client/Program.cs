using ArtifactsMMO.Client.Models;

Console.WriteLine("Starting Client!");


// Configuration
var username = "Erosion";
var password = "";

var targetCharacter = "Erosion";

var client = new HttpClient();
var commands = new Commands(client);
await commands.Authenticate(username, password); // Ideally we would only authenticate when our token expires, but the API does not return a TTL

var inator = new Inator(commands);

await inator.FightChickens(targetCharacter);

Console.WriteLine("Finished Commands. Are ya winning, son?");