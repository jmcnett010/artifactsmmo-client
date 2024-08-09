using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class CharacterResponse 
{
    [JsonPropertyName("data")]
    public required Character Character { get; init; }

    public static CharacterResponse Parse(string responseJson)
    {
        return JsonSerializer.Deserialize<CharacterResponse>(responseJson) ?? new CharacterResponse{ Character = new Character{Name = "", Level = 0, InventoryMax = 0, Inventory = [] }};
    }

    public string ToJson() 
    {
        return JsonSerializer.Serialize(this);
    }
}