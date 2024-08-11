namespace ArtifactsMMO.Client.Models;

/// <summary>
/// This class combines multiple commands to create repeatable actions
/// </summary>
public class Inator(Commands commands, List<Coordinates> map, List<CraftableItem> allItems)
{
    public async Task FightChickens()
    {
        while (true)
        {
            try
            {
                var coords = commands.FindLocation(map, "chicken");
                await commands.Move(coords);

                while (true)
                {
                    var result = await commands.Attack();

                    var combatResult = result.Data;
                    Console.WriteLine($"FightResult: {combatResult.Fight.Result}");
                    Console.WriteLine($"RemainingSeconds: {combatResult.Cooldown.RemainingSeconds}");

                    // Wait until the cooldown is up
                    Thread.Sleep(TimeSpan.FromSeconds(combatResult.Cooldown.RemainingSeconds + 1));

                    // Check if inventory is full
                    var characterData = await commands.GetCharacter();
                    if (characterData.IsInventoryFull()) {
                        Console.WriteLine($"Inventory is full, dumping contents in bank.");
                        await DepositInventory();
                        await commands.Move(Locations.LocationLookup["Chicken Coop"]);
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

    public async Task DepositInventory()
    {
        // Go to bank
        var moveCd = await commands.Move(Locations.LocationLookup["Bank"]);
        Thread.Sleep(TimeSpan.FromSeconds(moveCd.TotalSeconds + 1));

        var characters = await commands.GetCharacter();
        var iventory = characters.Inventory.Where(i => i.Quantity > 0).ToList();

        foreach(var item in iventory) 
        {
            var depositCd = await commands.DepositItem(item);
            Thread.Sleep(TimeSpan.FromSeconds(depositCd.TotalSeconds));
        }
    }

    public async Task CraftItem(CraftableItem item)
    {
        // var craftingLocation = item.Craft.Skill;


        // Travel to bank

        // Get materials from bank

        // Go to the correct crafting station


        // Execute the craft
    }
}