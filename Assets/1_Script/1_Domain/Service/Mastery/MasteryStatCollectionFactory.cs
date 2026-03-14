using System.Collections.Generic;

public record MasteryMultiplier(int Attack, int Defense, int Speed);

public class MasteryStatCollectionFactory
{
    readonly MasteryMultiplier _multiplier;

    public MasteryStatCollectionFactory(MasteryMultiplier multiplier)
    {
        _multiplier = multiplier;
    }

    public MasteryStatCollection Create(MasteryBoardCollection boardCollection)
    {
        var masteries = new List<ChampionMastery>();

        foreach (var kvp in boardCollection.AllBoards)
        {
            int championId = kvp.Key;
            MasteryBoard board = kvp.Value;

            var statData = new ChampionStatData(
                board.AttackLevel * _multiplier.Attack,
                board.DefenseLevel * _multiplier.Defense,
                board.SpeedLevel * _multiplier.Speed
            );

            masteries.Add(new ChampionMastery(championId, statData));
        }

        return new MasteryStatCollection(masteries);
    }
}