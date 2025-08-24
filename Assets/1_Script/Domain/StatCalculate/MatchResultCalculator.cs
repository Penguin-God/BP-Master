
using System.Collections.Generic;

public readonly struct MatchResult
{
    public readonly int BlueScore;
    public readonly int RedScore;
    public readonly Team Winner;

    public MatchResult(int blueScore, int redScore, Team winner)
    {
        BlueScore = blueScore;
        RedScore = redScore;
        Winner = winner;
    }
}

public class MatchResultCalculator
{ 
    public MatchResult CalculateResult(IEnumerable<ChampionStatData> blue, IEnumerable<ChampionStatData> red)
    {
        return default(MatchResult);
    }
}
