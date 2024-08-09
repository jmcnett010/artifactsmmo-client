namespace ArtifactsMMO.Client.Models;

/// <summary>
/// This class combines multiple commands to create repeatable actions
/// </summary>
public class Inator(Commands commands)
{
    public async Task FightChickens(string character)
    {
        while (true)
        {
            try
            {
                await commands.Move(character, Locations.LocationLookup["ChickenCoop"]);

                while (true)
                {
                    var result = await commands.Attack(character);

                    var combatResult = result.Data;
                    Console.WriteLine($"FightResult: {combatResult.Fight.Result}");
                    Console.WriteLine($"RemainingSeconds: {combatResult.Cooldown.RemainingSeconds}");

                    // Wait until the cooldown is up
                    Thread.Sleep(TimeSpan.FromSeconds(combatResult.Cooldown.RemainingSeconds + 1));

                    // Check if inventory is full
                    var characterData = await commands.GetCharacter(character);
                    if (characterData.IsInventoryFull()) {
                        Console.WriteLine($"Inventory is full, dumping contents in bank.");
                        await DepositInventory(character);
                        await commands.Move(character, Locations.LocationLookup["ChickenCoop"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                // If we get error responses, try again in a couple of minutes.
                Thread.Sleep(TimeSpan.FromSeconds(120));
            }
        }
    }

    public async Task DepositInventory(string character)
    {
        // Go to bank
        await commands.Move(character, Locations.LocationLookup["Bank"]);

        var characters = await commands.GetCharacter(character);
        var iventory = characters.Inventory.Where(i => i.Quantity > 0).ToList();

        foreach(var item in iventory) {
            var cooldown = await commands.DepositItem(character, item);
            Thread.Sleep(TimeSpan.FromSeconds(cooldown.TotalSeconds + 1));
        }
    }
}