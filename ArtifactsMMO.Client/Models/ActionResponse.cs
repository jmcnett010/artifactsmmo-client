using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class ActionResponse
{
    [JsonPropertyName("data")]
    public Cooldown Data { get; set; } = new Cooldown();

    public static ActionResponse Parse(string responseJson)
    {
        return JsonSerializer.Deserialize<ActionResponse>(responseJson) ?? new ActionResponse();
    }
}
