using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class CraftResponse
{
    [JsonPropertyName("data")]
    public CraftResponseData Data { get; set; } = new CraftResponseData();

    public static CraftResponse Parse(string responseJson)
    {
        return JsonSerializer.Deserialize<CraftResponse>(responseJson) ?? new CraftResponse();
    }
}

public class CraftResponseData
{
    [JsonPropertyName("cooldown")]
    public Cooldown Cooldown { get; set; } = new Cooldown();

    [JsonPropertyName("details")]
    public Skill Details { get; set; } = new Skill();

    [JsonPropertyName("character")]
    public Character Character { get; set; } = new Character();
}
