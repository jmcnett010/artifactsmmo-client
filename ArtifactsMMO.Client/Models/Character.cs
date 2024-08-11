using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class Character 
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("inventory_max_items")]
    public int InventoryMax { get; set; }

    [JsonPropertyName("inventory")]
    public List<InventoryItem> Inventory { get; set; } = [];

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