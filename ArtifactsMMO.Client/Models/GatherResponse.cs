using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class GatherResponse
{
    [JsonPropertyName("data")]
    public GatherData Data { get; set; } = new GatherData();

    public static GatherResponse Parse(string responseJson)
    {
        return JsonSerializer.Deserialize<GatherResponse>(responseJson) ?? new GatherResponse();
    }
}


public class GatherData
{
    [JsonPropertyName("cooldown")]
    public Cooldown Cooldown { get; set; } = new Cooldown();

    [JsonPropertyName("details")]
    public Skill Details { get; set; } = new Skill();

    [JsonPropertyName("character")]
    public Character Character { get; set; } = new Character();
}
