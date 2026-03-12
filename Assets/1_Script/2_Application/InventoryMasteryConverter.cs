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
        // 인벤토리에 해당 챔피언의 보드가 있다면 레벨 * 배율을 계산하여 반환합니다.
        if (_inventory.Boards.TryGetValue(championId, out var board))
        {
            return new ChampionStatData(
                board.AttackLevel * _multiplier.Attack,
                board.DefenseLevel * _multiplier.Defense,
                board.SpeedLevel * _multiplier.Speed
            );
        }

        // 보드가 없다면 빈 스탯(0, 0, 0)을 반환합니다.
        return default;
    }
}