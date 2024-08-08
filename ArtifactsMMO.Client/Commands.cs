using System.Runtime.InteropServices;
using System.Text;

namespace ArtifactsMMO.Client.Models;

public class Commands(HttpClient client)
{
    private HttpClient Client { get; set; } = client;
    private readonly string Url = "https://api.artifactsmmo.com";
    private AuthenticationToken AuthenticationToken { get; set; } = new AuthenticationToken();

    public async Task Authenticate(string username, string password)
    {
        Console.WriteLine($"Getting access token...");

        var bytes = Encoding.UTF8.GetBytes($"{username}:{password}");
        var base64Credentials = Convert.ToBase64String(bytes);

        Console.WriteLine(base64Credentials);

        var requestUrl = new Uri($"{Url}/token/");

        Console.WriteLine(requestUrl.ToString());

        var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

        request.Headers.Add("Accept", $"application/json");
        request.Headers.Add("Authorization", $"Basic {base64Credentials}");

        var response = await Client.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        Console.WriteLine(responseContent);

        AuthenticationToken = AuthenticationToken.ParseResponse(responseContent);
    }

    public async Task Move(string character, Coordinates coords)
    {
        Console.WriteLine($"Moving {character} to {coords}");

        var requestUrl = new Uri($"{Url}/my/{character}/action/move");

        Console.WriteLine(requestUrl.ToString());

        using StringContent jsonContent = new(
            coords.ToJson(),
            Encoding.UTF8,
            "application/json");

        // Build Request
        var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
        {
            Content = jsonContent
        };

        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("Authorization", $"{AuthenticationToken}");

        var response = await Client.SendAsync(request);

        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }

    public async Task<CombatResults> Attack(string character)
    {
        Console.WriteLine($"Attacking with {character}");

        var requestUrl = new Uri($"{Url}/my/{character}/action/fight");

        // Build Request
        var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("Authorization", $"{AuthenticationToken}");

        var response = await Client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        return CombatResults.Parse(responseBody);
    }

    public async Task Gather()
    {

    }

    public async Task Craft() {

    }
}