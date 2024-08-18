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

    [JsonPropertyName("mining_level")]
    public int MiningLevel { get; set; } 

    [JsonPropertyName("woodcutting_level")]
    public int WoodcuttingLevel { get; set; } 

    [JsonPropertyName("fishing_level")]
    public int FishingLevel { get; set; } 

    [JsonPropertyName("weaponcrafting_level")]
    public int WeaponcraftingLevel { get; set; } 

    [JsonPropertyName("gearcrafting_level")]
    public int GearcraftingLevel { get; set; } 

    [JsonPropertyName("jewelrycrafting_level")]
    public int JewelrycraftingLevel { get; set; } 

    [JsonPropertyName("cooking_level")]
    public int CookingLevel { get; set; } 

    public override string ToString() 
    {
        return $"{Name}";
    }

    public bool IsInventoryFull() {
        var iventoryCount = Inventory.Sum(i => i.Quantity);
        return iventoryCount + 10 >= InventoryMax;
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