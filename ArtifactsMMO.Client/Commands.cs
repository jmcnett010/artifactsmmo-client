using System.Text;
using System.Text.Json;

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

        public async Task<Cooldown> WithdrawItem(InventoryItem item)
    {
        Console.WriteLine($"[{Character}] is withdrawing {item}");

        var requestUrl = new Uri($"{Url}/my/{Character}/action/bank/withdraw");

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
        Console.WriteLine($"Pulling Character [{Character}]..");

        var requestUrl = new Uri($"{Url}/characters/{Character}");

        // Build Request
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

        var response = await Client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        var parsedCharacterResponse = CharacterResponse.Parse(responseBody);
        return parsedCharacterResponse.Character;
    }

    public async Task<List<Coordinates>> CheckMap()
    {
        Console.WriteLine($"Pulling map data...");
        var page = 1;
        var size = 10;
        var coordinates = new List<Coordinates>();
        var pages = 100;

        while (page != pages) {
            var requestUrl = new Uri($"{Url}/maps?page={page}&size={size}");

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            var response = await Client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            MapResponse parsedResponse;
            try
            {
                parsedResponse = MapResponse.Parse(responseBody);
                coordinates.AddRange(parsedResponse.Coordinates ?? []);
                pages = parsedResponse.Pages;
                page++;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        return coordinates;
    }

    /// <summary>
    /// Ideally, this would find the closest location.
    /// </summary>
    /// <param name="content">The map content we are looking for</param>
    /// <returns></returns>
    public Coordinates FindLocation(List<Coordinates> map, string content)
    {
        var availableOptions = map.Where(coords => (coords.Content?.Code ?? "") == content).ToList();

        // TODO - Get closest option;
        return availableOptions.First();
    }

    public async Task<List<CraftableItem>> CheckAllItems()
    {
        Console.WriteLine($"Pulling item data...");
        var page = 1;
        var size = 10;
        var items = new List<CraftableItem>();
        var pages = 100;

        while (page != pages) {
            var requestUrl = new Uri($"{Url}/items?page={page}&size={size}");

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            var response = await Client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            AllItemsResponse parsedResponse;
            try
            {
                parsedResponse = AllItemsResponse.Parse(responseBody);
                items.AddRange(parsedResponse.CraftableItems ?? []);
                pages = parsedResponse.Pages;
                page++;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        return items;
    }

    public async Task<List<Monster>> CheckAllMonsters()
    {
        Console.WriteLine($"Pulling monster data...");
        var page = 1;
        var size = 10;
        var items = new List<Monster>();
        var pages = 100;

        while (page != pages) {
            var requestUrl = new Uri($"{Url}/monsters?page={page}&size={size}");

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            var response = await Client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            AllMonstersResponse parsedResponse;
            try
            {
                parsedResponse = AllMonstersResponse.Parse(responseBody);
                items.AddRange(parsedResponse.Monsters ?? []);
                pages = parsedResponse.Pages;
                page++;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        return items;
    }

        public async Task<List<Resource>> CheckAllResources()
    {
        Console.WriteLine($"Pulling resource data...");
        var page = 1;
        var size = 10;
        var items = new List<Resource>();
        var pages = 100;

        while (page != pages) {
            var requestUrl = new Uri($"{Url}/resources?page={page}&size={size}");

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            var response = await Client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            AllResourcesResponse parsedResponse;
            try
            {
                parsedResponse = AllResourcesResponse.Parse(responseBody);
                items.AddRange(parsedResponse.Resources ?? []);
                pages = parsedResponse.Pages;
                page++;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        return items;
    }

    public async Task<GatherData> Gather()
    {
        Console.WriteLine($"Gathering with {Character}");
        var requestUrl = new Uri($"{Url}/my/{Character}/action/gathering");

        // Build Request
        var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

        var response = await Client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        var parsedResponse = GatherResponse.Parse(responseBody);
        return parsedResponse.Data ?? new GatherData();
    }

    public async Task<CraftResponseData> Craft(string code, int quantity)
    {
        Console.WriteLine($"Crafting {code}");
        var requestUrl = new Uri($"{Url}/my/{Character}/action/crafting");

        using StringContent jsonContent = new(
            JsonSerializer.Serialize(new { code, quantity}),
            Encoding.UTF8,
            "application/json");

        // Build Request
        var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
        {
            Content = jsonContent
        };

        var response = await Client.SendAsync(request);

        var responseBody = await response.Content.ReadAsStringAsync();
        var parsedResponse = CraftResponse.Parse(responseBody);

        return parsedResponse.Data ?? new CraftResponseData();
    }

    public async Task<Cooldown> Doff(string slot)
    {
        Console.WriteLine($"Doffing {slot}");
        var requestUrl = new Uri($"{Url}/my/{Character}/action/unequip");

        using StringContent jsonContent = new(
            JsonSerializer.Serialize(new { slot}),
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

        public async Task<Cooldown> Don(CraftableItem item)
    {
        Console.WriteLine($"Donning {item.Code} on {item.Type}");
        var requestUrl = new Uri($"{Url}/my/{Character}/action/equip");

        using StringContent jsonContent = new(
            JsonSerializer.Serialize(item),
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