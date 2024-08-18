using System.Text.Json;

namespace ArtifactsMMO.Client.Models;

/// <summary>
/// This class combines multiple commands to create repeatable actions
/// </summary>
public class Inator(Commands commands, List<Coordinates> map, List<CraftableItem> allItems, List<Resource> allResources, List<Monster> allMonsters)
{
    public async Task GetItemsFromMonster(CraftableItem item, int quantity)
    {
        // Lookup what drops the item we care about. 
        var potentialTargetMonsters = allMonsters.Where(monster => monster.Drops.Any(d => d.Code == item.Code));
        var lowestLevelMonster = potentialTargetMonsters.OrderBy(m => m.Level).First();

        var monsterCoords = commands.FindLocation(map, lowestLevelMonster.Code);
        var cd = await commands.Move(monsterCoords);
        Thread.Sleep(TimeSpan.FromSeconds(cd.RemainingSeconds + 1));

        var gatheredQuantity = 0;
        while (gatheredQuantity < quantity)
        {
            var result = await commands.Attack();

            var combatResult = result.Data;
            Console.WriteLine($"FightResult: {combatResult.Fight.Result}");
            Console.WriteLine($"RemainingSeconds: {combatResult.Cooldown.RemainingSeconds}");

            // Wait until the cooldown is up
            Thread.Sleep(TimeSpan.FromSeconds(combatResult.Cooldown.RemainingSeconds + 1));

            gatheredQuantity += combatResult.Fight.Drops.Count(i => i.Code == item.Code);

            // Check if inventory is full
            var characterData = await commands.GetCharacter();
            if (characterData.IsInventoryFull())
            {
                Console.WriteLine($"Inventory is full, dumping contents in bank.");
                await DepositInventory();
                var moveCd = await commands.Move(monsterCoords);
                Thread.Sleep(TimeSpan.FromSeconds(moveCd.RemainingSeconds + 1));
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

        foreach (var item in iventory)
        {
            var depositCd = await commands.DepositItem(item);
            Thread.Sleep(TimeSpan.FromSeconds(depositCd.TotalSeconds));
        }
    }

    public async Task WithdrawBankItem(InventoryItem item)
    {
        // Go to bank
        var moveCd = await commands.Move(Locations.LocationLookup["Bank"]);
        Thread.Sleep(TimeSpan.FromSeconds(moveCd.TotalSeconds + 1));

        var withdrawCd = await commands.WithdrawItem(item);
        Thread.Sleep(TimeSpan.FromSeconds(withdrawCd.TotalSeconds));
    }

    public async Task GatherTargetItem(CraftableItem item, int quantity)
    {
        var potentialTargetResource = allResources.Where(monster => monster.Drops.Any(d => d.Code == item.Code));
        var lowestLevelResource = potentialTargetResource.OrderBy(m => m.Level).First();

        var coords = commands.FindLocation(map, lowestLevelResource.Code);
        var cd = await commands.Move(coords);
        Thread.Sleep(TimeSpan.FromSeconds(cd.RemainingSeconds + 1));

        var gatheredQuantity = 0;
        while (gatheredQuantity < quantity)
        {
            var gatherResult = await commands.Gather();
            Console.WriteLine($"GatherResult: {JsonSerializer.Serialize(gatherResult)}");
            Console.WriteLine($"RemainingSeconds: {gatherResult.Cooldown.TotalSeconds}");
            Thread.Sleep(TimeSpan.FromSeconds(gatherResult.Cooldown.RemainingSeconds + 1));

            gatheredQuantity += gatherResult.Details.Items.Count(i => i.Code == item.Code);

            // Wait until the cooldown is up
            Thread.Sleep(TimeSpan.FromSeconds(gatherResult.Cooldown.TotalSeconds + 1));

            // Check if inventory is full
            var characterData = await commands.GetCharacter();
            if (characterData.IsInventoryFull())
            {
                Console.WriteLine($"Inventory is full, dumping contents in bank.");
                await DepositInventory();
                var bankDumpCd = await commands.Move(coords);
                Thread.Sleep(TimeSpan.FromSeconds(bankDumpCd.RemainingSeconds + 1));
            }
        }

        Console.WriteLine($"Got {quantity} {item.Code}!");
    }

    /// <summary>
    /// Crafts a quantity of the provided item, given the materials are in your inventory
    /// </summary>
    /// <param name="item"></param>
    /// <param name="quantity"></param>
    /// <returns>Returns the number created</returns>
    public async Task<int> CraftItem(CraftableItem item, int quantity)
    {
        if (item.Craft == null) 
        {
            return 0;
        }

        while (true)
        {
            var coords = commands.FindLocation(map, item.Craft.Skill);
            var moveCd = await commands.Move(coords);
            Thread.Sleep(TimeSpan.FromSeconds(moveCd.RemainingSeconds + 1));

            var craftResponse = await commands.Craft(item.Code, quantity);
            Thread.Sleep(TimeSpan.FromSeconds(craftResponse.Cooldown.RemainingSeconds + 1));

            var quantityCreated = craftResponse.Details.Items.Where(i => i.Code == item.Code).Sum(i => i.Quantity);

            Console.WriteLine($"Got {quantityCreated} {item.Name}!");
            return quantityCreated;
        }
    }

    public async Task CraftItemFromBank(CraftableItem item, int quantity)
    {
        // Withdraw requirements
        foreach (var requirementItem in item.Craft.Items)
        {
            if (requirementItem == null)
            {
                continue;
            }

            await WithdrawBankItem(requirementItem);
        }

        await CraftItem(item, quantity);

        return;
    }

    public async Task SwapItemIntoSlot(CraftableItem item)
    {
        var doffCd = await commands.Doff(item.Type);
        Thread.Sleep(TimeSpan.FromSeconds(doffCd.TotalSeconds));

        var character = await commands.GetCharacter();
        var staff = character.Inventory.First(i => i.Code == item.Code);

        var donCd = await commands.Don(item);
        Thread.Sleep(TimeSpan.FromSeconds(donCd.TotalSeconds));
    }

    public async Task ProcureRequiredItemsAndCraft(CraftableItem item, int quantity = 1)
    {
        var currentQuantity = 0;

        // Check if we have the item already
        currentQuantity = (await commands.GetCharacter()).Inventory.FirstOrDefault(i => i.Code == item.Code)?.Quantity ?? 0;
        while (currentQuantity < quantity)
        {
            Console.WriteLine($"Attempting to procure {item.Code}. Need {quantity}, have {currentQuantity}.");

            if (item.Craft == null)
            {
                // Get any items that are unable to be crafted
                await ProcureItems(item, quantity - currentQuantity);

                // Ensure it is in our inventory
                currentQuantity = (await commands.GetCharacter()).Inventory.FirstOrDefault(i => i.Code == item.Code)?.Quantity ?? 0;
                return;
            }

            // If the item has requirements to craft, then keep walking the tree. 
            foreach (var requiredItem in item.Craft.Items)
            {
                if (requiredItem == null)
                {
                    continue;
                }

                // Lookup how to get item, then recursively get them all
                var itemDefinition = allItems.First(i => i.Code == requiredItem.Code);
                await ProcureRequiredItemsAndCraft(itemDefinition, (requiredItem.Quantity - currentQuantity) * quantity);
            }

            // By this point in our recursive tree, we can just craft any 
            // items we have the materials for
            await ProcureItems(item, quantity - currentQuantity);

            // Ensure it is in our inventory
            currentQuantity = (await commands.GetCharacter()).Inventory.FirstOrDefault(i => i.Code == item.Code)?.Quantity ?? 0;
        }
    }

    public async Task ProcureItems(CraftableItem item, int quantity)
    {
        // Is it craftable? Then make it
        if (item.Craft != null)
        {
            Console.WriteLine($"Attempting to craft {quantity} {item.Code}.");
            await CraftItem(item, quantity);
            return;
        }

        // Do you get it through violence?
        if (item.Subtype == "mob")
        {
            Console.WriteLine($"Attempting to loot {quantity} {item.Code}.");
            await GetItemsFromMonster(item, quantity);
            return;
        }

        // Get your hands dirty
        if (item.Type == "resource")
        {
            Console.WriteLine($"Attempting to gather {quantity} {item.Code}.");
            await GatherTargetItem(item, quantity);
        }
    }

    public async Task GrindToSkill(Craft targetCraft)
    {
        // Check skill
        var currentSkill = 0;
        var character =  await commands.GetCharacter();

        switch (targetCraft.Skill)
        {
            case "mining":
                currentSkill = character.MiningLevel;
                break;
            case "woodcutting":
                currentSkill = character.WoodcuttingLevel;
                break;
            case "fishing":
                currentSkill = character.FishingLevel;
                break;
            case "weaponcrafting":
                currentSkill = character.WeaponcraftingLevel;
                break;
            case "gearcrafting":
                currentSkill = character.GearcraftingLevel;
                break;
            case "jewelrycrafting":
                currentSkill = character.JewelrycraftingLevel;
                break;
            case "cooking":
                currentSkill = character.CookingLevel;
                break;
            default:
                currentSkill = 0;
                break;
        }

        // Grind skill
        // step 1 - lookup top craftable item in skill
        // step 2 - procure and craft item
        // step 3 - repeat this method
    }
}