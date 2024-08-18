using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client.Models;

public class Drop 
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = "";
    [JsonPropertyName("rate")]
    public int Rate { get; set; }
    [JsonPropertyName("min_quantity")]
    public int MinimumQuantity { get; set; }
    [JsonPropertyName("max_quantity")]
    public int MaximumQuantity { get; set; }

    public string ToJson() 
    {
        return JsonSerializer.Serialize(this);
    }
}
