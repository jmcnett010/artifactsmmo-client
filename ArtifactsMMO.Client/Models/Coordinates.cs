using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class Coordinates 
{
    [JsonPropertyName("x")]
    public required int X { get; init; }
    [JsonPropertyName("y")]
    public required int Y { get; init; }

    public override string ToString() 
    {
        return $"({X},{Y})";
    }

    public string ToJson() 
    {
        return JsonSerializer.Serialize(this);
    }
}