using System.Text;

namespace ArtifactsMMO.Client.Models;

public class Commands(HttpClient client, string character)
{
    private HttpClient Client { get; set; } = client;
    private readonly string Url = "https://api.artifactsmmo.com";
    private string Character { get; } = character;

    public async Task<AuthenticationToken> Authenticate(string username, string password)
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

        return AuthenticationToken.ParseResponse(responseContent);
    }

    public async Task<Cooldown> Move(Coordinates coords)
    {
        Console.WriteLine($"Moving {Character} to {coords}");

        var requestUrl = new Uri($"{Url}/my/{Character}/action/move");

        using StringContent jsonContent = new(
            coords.ToJson(),
            Encoding.UTF8,
            "application/json");

        // Build Request
        var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
        {
            Content = jsonContent
        };

        // request.Headers.Add("Accept", "application/json");
        // request.Headers.Add("Authorization", $"{AuthenticationToken}");

        var response = await Client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        var parsedResponse = ActionResponse.Parse(responseBody);
        return parsedResponse.Data.Cooldown ?? new Cooldown();
    }

    public async Task<CombatResults> Attack()
    {
        Console.WriteLine($"Attacking with {Character}");
        var requestUrl = new Uri($"{Url}/my/{Character}/action/fight");

        // Build Request
        var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

        var response = await Client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        return CombatResults.Parse(responseBody);
    }

    public async Task<Cooldown> DepositItem(InventoryItem item)
    {
        Console.WriteLine($"[{Character}] is depositing {item}");

        var requestUrl = new Uri($"{Url}/my/{Character}/action/bank/deposit");

        using StringContent jsonContent = new(
            item.ToJson(),
            Encoding.UTF8,
            "application/json");

        // Build Request
        var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
        {
            Content = jsonContent
        };

        var response = await Client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        var parsedResponse = ActionResponse.Parse(responseBody);
        return parsedResponse.Data.Cooldown ?? new Cooldown();
    }

    public async Task<Character> GetCharacter()
    {
        Console.WriteLine($"Pulling Characters..");

        var requestUrl = new Uri($"{Url}/Characters/{Character}");

        // Build Request
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

        var response = await Client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        var parsedCharacterResponse = CharacterResponse.Parse(responseBody);
        return parsedCharacterResponse.Character;
    }

    public async Task CheckMap()
    {
        Console.WriteLine($"Gathering with {Character}");
        var requestUrl = new Uri($"{Url}/my/{Character}/maps");

        // Build Request
        var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

        var response = await Client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        // var parsedResponse = ActionResponse.Parse(responseBody);
        // return parsedResponse.Data.Cooldown ?? new Cooldown();
    }

    public async Task<List<CraftableItem>> CheckAllItems()
    {
        var requestUrl = new Uri($"{Url}/items");

        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

        var response = await Client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

AllItemsResponse parsedResponse;
try
{
    parsedResponse = AllItemsResponse.Parse(responseBody);
}
catch (System.Exception ex)
{
    Console.WriteLine(ex.Message);
    throw;
}


        return parsedResponse.CraftableItems ?? [];
    }

    public async Task<Cooldown> Gather()
    {
        Console.WriteLine($"Gathering with {Character}");
        var requestUrl = new Uri($"{Url}/my/{Character}/action/gathering");

        // Build Request
        var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

        var response = await Client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        var parsedResponse = ActionResponse.Parse(responseBody);
        return parsedResponse.Data.Cooldown ?? new Cooldown();
    }

    public async Task<Cooldown> Craft(InventoryItem item)
    {
        var requestUrl = new Uri($"{Url}/my/{Character}/action/crafting");

        using StringContent jsonContent = new(
            item.ToJson(),
            Encoding.UTF8,
            "application/json");

        // Build Request
        var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
        {
            Content = jsonContent
        };

        var response = await Client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        var parsedResponse = ActionResponse.Parse(responseBody);
        return parsedResponse.Data.Cooldown ?? new Cooldown();
    }
}