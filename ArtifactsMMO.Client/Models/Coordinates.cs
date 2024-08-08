using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class Coordinates
{
    [JsonPropertyName("x")]
    public int X { get; set; }
    [JsonPropertyName("y")]
    public int Y { get; set; }

    public Coordinates(int x, int y)
    {
        X = x;
        Y = y;
    }

    public string ToJson() 
    {
        return JsonSerializer.Serialize(this);
    }

    public override string ToString() 
    {
        return $"({X},{Y})";
    }
}