
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
    readonly TeamScoreCalculator scoreCalculator;
    public MatchResultCalculator(TeamScoreCalculator teamScoreCalculator) 
    {
        this.scoreCalculator = teamScoreCalculator;
    }

    public MatchResult CalculateResult(IEnumerable<ChampionStatData> blue, IEnumerable<ChampionStatData> red)
    {
        int blueScore = scoreCalculator.CalculateScore(blue);
        int redScore = scoreCalculator.CalculateScore(red);

        Team winner;
        if (blueScore == redScore) winner = Team.All;
        else if (blueScore > redScore) winner = Team.Blue;
        else winner = Team.Red;

        return new MatchResult(blueScore, redScore, winner);
    }
}
