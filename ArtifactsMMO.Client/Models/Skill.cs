using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class Skill 
{
    [JsonPropertyName("xp")]
    public int Experience { get; set; }
    
    [JsonPropertyName("items")]
    public List<InventoryItem> Items { get; set; } = [];

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}
