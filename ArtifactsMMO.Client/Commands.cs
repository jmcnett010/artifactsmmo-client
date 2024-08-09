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

    public async Task<Cooldown> DepositItem(string character, InventoryItem item)
    {
        Console.WriteLine($"[{character}] is depositing {item}");

        var requestUrl = new Uri($"{Url}/my/{character}/action/bank/deposit");

        using StringContent jsonContent = new(
            item.ToJson(),
            Encoding.UTF8,
            "application/json");

        // Build Request
        var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
        {
            Content = jsonContent
        };
        
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("Authorization", $"{AuthenticationToken}");

        Console.WriteLine(item.ToJson());

        var response = await Client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine(responseBody);
        var parsedResponse = ActionResponse.Parse(responseBody);
        return parsedResponse.Data;
    }

    public async Task<Character> GetCharacter(string character)
    {
        Console.WriteLine($"Pulling characters..");

        var requestUrl = new Uri($"{Url}/characters/{character}");

        // Build Request
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("Authorization", $"{AuthenticationToken}");

        var response = await Client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        var parsedCharacterResponse = CharacterResponse.Parse(responseBody);
        return parsedCharacterResponse.Character;
    }

    public async Task Gather()
    {

    }

    public async Task Craft() {

    }
}