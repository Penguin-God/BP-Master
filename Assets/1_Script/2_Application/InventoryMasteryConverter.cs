public class InventoryMasteryConverter : IMasteryStatProvider
{
    readonly MasteryInventory _inventory;
    readonly MasteryMultiplier _multiplier;

    public InventoryMasteryConverter(MasteryInventory inventory, MasteryMultiplier multiplier)
    {
        _inventory = inventory;
        _multiplier = multiplier;
    }

    public ChampionStatData GetMasteryStat(int championId)
    {
        if (_inventory.Boards.TryGetValue(championId, out var board))
        {
            return new ChampionStatData(
                board.AttackLevel * _multiplier.Attack,
                board.DefenseLevel * _multiplier.Defense,
                board.SpeedLevel * _multiplier.Speed
            );
        }

        return default;
    }
}