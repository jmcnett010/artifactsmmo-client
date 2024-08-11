using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class ActionResponse
{
    [JsonPropertyName("data")]
    public Data Data { get; set; } = new Data();

    public static ActionResponse Parse(string responseJson)
    {
        return JsonSerializer.Deserialize<ActionResponse>(responseJson) ?? new ActionResponse();
    }
}


public class Data
{
    [JsonPropertyName("cooldown")]
    public Cooldown Cooldown { get; set; } = new Cooldown();
}
