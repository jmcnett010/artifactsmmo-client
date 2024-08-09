using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class Character 
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }
    [JsonPropertyName("level")]
    public required int Level { get; init; }

    [JsonPropertyName("inventory_max_items")]
    public required int InventoryMax { get; init; }

    [JsonPropertyName("inventory")]
    public required List<InventoryItem> Inventory { get; init; }

    public override string ToString() 
    {
        return $"{Name}";
    }

    public bool IsInventoryFull() {
        var iventoryCount = Inventory.Sum(i => i.Quantity);
        return iventoryCount + 1 >= InventoryMax;
    }

    public static Character Parse(string responseJson)
    {
        return JsonSerializer.Deserialize<Character>(responseJson) ?? new Character{Name = "", Level = 0, InventoryMax = 0, Inventory = [] };
    }

    public string ToJson() 
    {
        return JsonSerializer.Serialize(this);
    }
}