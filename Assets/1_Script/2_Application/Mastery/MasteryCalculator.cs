using System.Linq;

public record MasteryMultiplier(int Attack, int Defense, int Speed);

public class MasteryCalculator
{
    readonly MasteryMultiplier _multiplier;
    public MasteryCalculator(MasteryMultiplier multiplier) => _multiplier = multiplier;

    public MasteryStatCollection Create(MasteryBoardCollection boardCollection) => new MasteryStatCollection(boardCollection.AllBoards.Select(x => new ChampionMastery(x.Key, CalculateStat(x.Value)))); 

    ChampionStatData CalculateStat(MasteryBoard board)
        => new ChampionStatData(board.AttackLevel * _multiplier.Attack, board.DefenseLevel * _multiplier.Defense, board.SpeedLevel * _multiplier.Speed);

    public static ChampionStatData CreateMasteryStat(MasteryMultiplier multiplier, MasteryBoard board)
        => new ChampionStatData(board.AttackLevel * multiplier.Attack, board.DefenseLevel * multiplier.Defense, board.SpeedLevel * multiplier.Speed);
}