using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class InventoryItem 
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = "";
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
    [JsonPropertyName("slot")]
    public int Slot { get; set; }

    public override string ToString() {
        return $"[Name: {Code}, Quantity: {Quantity}, Slot: {Slot}]";
    }
    public string ToJson() 
    {
        return JsonSerializer.Serialize(this);
    }

    public static InventoryItem FromCraftable(CraftableItem item, int quantity)
    {
        return new InventoryItem 
        {
            Code = item.Name,
            Quantity = quantity,
        };
    }
}
