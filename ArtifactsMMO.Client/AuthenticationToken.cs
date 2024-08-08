using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtifactsMMO.Client;

public class AuthenticationToken
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = "";

    public static AuthenticationToken ParseResponse(string responseJson)
    {
        return JsonSerializer.Deserialize<AuthenticationToken>(responseJson) ?? new AuthenticationToken();
    }

    public override string ToString() {
        return $"Bearer {Token}";
    }
}