using System.Collections.Generic;

public class InventoryMasteryConverter : IMasteryStatProvider
{
    readonly IReadOnlyDictionary<int, MasteryBoard> _boards;
    readonly MasteryMultiplier _multiplier;

    public InventoryMasteryConverter(IReadOnlyDictionary<int, MasteryBoard> boards, MasteryMultiplier multiplier)
    {
        _boards = boards;
        _multiplier = multiplier;
    }

    public ChampionStatData GetMasteryStat(int championId)
    {
        if (_boards.TryGetValue(championId, out var board))
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