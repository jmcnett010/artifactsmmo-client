using System.Text.Json;
using System.Text.Json.Serialization;
using ArtifactsMMO.Client.Models;

namespace ArtifactsMMO.Client.Models;

public class CraftableItem 
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("code")]
    public string Code { get; set; } = "";

    [JsonPropertyName("level")]
    public int Level { get; set; }
    
    [JsonPropertyName("type")]
    public string Type { get; set; } = "";

    [JsonPropertyName("subtype")]
    public string Subtype { get; set; } = "";

    [JsonPropertyName("description")]
    public string Description { get; set; } = "";

    [JsonPropertyName("effects")]
    public List<ItemEffect> Effects { get; set; } = [];

    [JsonPropertyName("craft")]
    public CraftableItem Craft { get; set; } = new CraftableItem();

    public string ToJson() 
    {
        return JsonSerializer.Serialize(this);
    }
}
