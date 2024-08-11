
namespace ArtifactsMMO.Client.Models;

public static class Locations 
{
    public static Dictionary<string, Coordinates> LocationLookup { get; } = new Dictionary<string, Coordinates>
    {

        ["Chicken Coop"] = new Coordinates{X = 0, Y = 1}, 
        ["Workshop Cooking"] = new Coordinates{X = 1, Y = 1},
        ["Workshop Weaponcrafting"] = new Coordinates{X = 2, Y = 1},
        ["Workshop Gearcrafting"] = new Coordinates{X = 3, Y = 1},
        ["Bank"] = new Coordinates{X = 4, Y = 1}, 
        ["Grand Exchange"] = new Coordinates{X = 5, Y = 1},
    };
}