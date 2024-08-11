using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class MapResponse 
{
    [JsonPropertyName("data")]
    public List<Coordinates> Coordinates { get; set; } = [];
    
    [JsonPropertyName("pages")]
    public int Pages { get; set; }

    public static MapResponse Parse(string responseJson)
    {
        return JsonSerializer.Deserialize<MapResponse>(responseJson) ?? new MapResponse();
    }
}