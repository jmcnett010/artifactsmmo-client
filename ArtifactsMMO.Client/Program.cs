using ArtifactsMMO.Client;

Console.WriteLine("Starting Client!");

using HttpClient client = new();


// Configuration
var username = "Erosion";
var password = "";
var character = "Erosion";


var commands = new Commands(client);

await commands.Authenticate(username, password);


var destination = new Coordinates(0, 1);

await commands.Move(character, destination);


Console.WriteLine("Finished Commands. Are ya winning, son?");