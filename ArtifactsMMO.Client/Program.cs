using System.Net.Http.Headers;
using ArtifactsMMO.Client.Models;

Console.WriteLine("Starting Client!");


// Configuration
var username = "Erosion";
var password = "";

var targetCharacter = "Erosion";

var client = new HttpClient();
var commands = new Commands(client, targetCharacter);
var token = await commands.Authenticate(username, password); // Ideally we would only authenticate when our token expires, but the API does not return a TTL

client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);


var items = await commands.CheckAllItems();
var map = await commands.CheckMap();

var inator = new Inator(commands, map, items);

await inator.FightChickens();



Console.WriteLine("Finished Commands. Are ya winning, son?");