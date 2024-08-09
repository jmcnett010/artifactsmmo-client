using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class InventoryItem 
{
    [JsonPropertyName("code")]
    public required string Name { get; init; }
    [JsonPropertyName("quantity")]
    public required int Quantity { get; init; }
    [JsonPropertyName("slot")]
    public required int Slot { get; init; }

    public override string ToString() {
        return $"[Name: {Name}, Quantity: {Quantity}, Slot: {Slot}]";
    }
    public string ToJson() 
    {
        return JsonSerializer.Serialize(this);
    }
}
