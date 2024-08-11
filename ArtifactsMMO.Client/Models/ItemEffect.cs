using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class ItemEffect 
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("value")]
    public int Value { get; set; }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}
