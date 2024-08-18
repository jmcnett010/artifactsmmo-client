using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class AllMonstersResponse 
{
    [JsonPropertyName("data")]
    public List<Monster> Monsters { get; set; } = [];
    
    [JsonPropertyName("pages")]
    public int Pages { get; set; }

    public static AllMonstersResponse Parse(string responseJson)
    {
        return JsonSerializer.Deserialize<AllMonstersResponse>(responseJson) ?? new AllMonstersResponse();
    }
}