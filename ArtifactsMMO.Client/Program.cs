using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Reflection;
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
var resources = await commands.CheckAllResources();
var monsters = await commands.CheckAllMonsters();

var inator = new Inator(commands, map, items, resources, monsters);

// var sword = items.Where(item => item.Code == "iron_sword").First();
// var coat = items.Where(item => item.Code == "feather_coat").First();
var helm = items.Where(item => item.Code == "copper_helmet").First();
// var boots = items.Where(item => item.Code == "copper_boots").First();
var dagger = items.Where(item => item.Code == "copper_dagger").First();
var ring = items.Where(item => item.Code == "copper_ring").First();


await inator.ProcureRequiredItemsAndCraft(ring);
await inator.SwapItemIntoSlot(ring);

// Lets level skills
for (int i = 0; i < 100; i++) {
    await inator.ProcureRequiredItemsAndCraft(dagger);
    await inator.DepositInventory();
    await inator.ProcureRequiredItemsAndCraft(ring);
    await inator.DepositInventory();
    await inator.ProcureRequiredItemsAndCraft(helm);
    await inator.DepositInventory();
}



Console.WriteLine("Finished Commands. Are ya winning, son?");

