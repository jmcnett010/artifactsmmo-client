using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class AllResourcesResponse 
{
    [JsonPropertyName("data")]
    public List<Resource> Resources { get; set; } = [];
    
    [JsonPropertyName("pages")]
    public int Pages { get; set; }

    public static AllResourcesResponse Parse(string responseJson)
    {
        return JsonSerializer.Deserialize<AllResourcesResponse>(responseJson) ?? new AllResourcesResponse();
    }
}