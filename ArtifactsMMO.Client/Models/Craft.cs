using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class Craft 
{
    /// <summary>
    /// weaponcrafting
    /// gearcrafting
    /// jewelrycrafting
    /// cooking
    /// woodcutting
    /// mining
    /// </summary>
    [JsonPropertyName("skill")]
    public string Skill { get; set; } = "";

    [JsonPropertyName("level")]
    public int Level { get; set; }
    
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("items")]
    public List<InventoryItem> Items { get; set; } = [];

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}
