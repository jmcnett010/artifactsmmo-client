using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class AllItemsResponse 
{
    [JsonPropertyName("data")]
    public List<CraftableItem> CraftableItems { get; set; } = [];
    
    [JsonPropertyName("pages")]
    public int Pages { get; set; }

    public static AllItemsResponse Parse(string responseJson)
    {
        return JsonSerializer.Deserialize<AllItemsResponse>(responseJson) ?? new AllItemsResponse();
    }
}
