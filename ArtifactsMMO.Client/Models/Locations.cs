
namespace ArtifactsMMO.Client.Models;

public static class Locations 
{
    public static Dictionary<string, Coordinates> LocationLookup { get; } = new Dictionary<string, Coordinates>
    {

        ["ChickenCoop"] = new Coordinates{X = 0, Y = 1}, 
        ["Bank"] = new Coordinates{X = 4, Y = 1}, 
    };
}