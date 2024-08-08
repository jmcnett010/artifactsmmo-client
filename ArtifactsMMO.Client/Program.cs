using ArtifactsMMO.Client.Models;

Console.WriteLine("Starting Client!");


// Configuration
var username = "Erosion";
var password = "";

var inator = new Inator(username, password);

while (true) {
    try
    {
        await inator.FightChickens("Erosion");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);

        // If we get error responses, try again in a couple of minutes.
        Thread.Sleep(TimeSpan.FromSeconds(120));
    }
}

Console.WriteLine("Finished Commands. Are ya winning, son?");