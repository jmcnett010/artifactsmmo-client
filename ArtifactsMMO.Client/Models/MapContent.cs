using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class MapContent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "";
    [JsonPropertyName("code")]
    public string Code { get; set; } = "";
}
