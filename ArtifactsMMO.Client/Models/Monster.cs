using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class Monster 
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("code")]
    public string Code { get; set; } = "";

    [JsonPropertyName("level")]
    public int Level { get; set; }
    
    [JsonPropertyName("hp")]
    public int Hp { get; set; }

    [JsonPropertyName("drops")]
    public List<Drop> Drops { get; set; } = [];


    public string ToJson() 
    {
        return JsonSerializer.Serialize(this);
    }
}
