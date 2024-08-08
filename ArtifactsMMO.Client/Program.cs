using ArtifactsMMO.Client.Models;

Console.WriteLine("Starting Client!");


// Configuration
var username = "Erosion";
var password = "";

var inator = new Inator(username, password);

await inator.FightChickens("Erosion");

Console.WriteLine("Finished Commands. Are ya winning, son?");