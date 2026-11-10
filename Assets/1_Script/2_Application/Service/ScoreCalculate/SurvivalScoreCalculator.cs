using System.Linq;

public static class SurvivalScoreCalculator
{
    public static MatchResult Calculate(SlotStorage<ChampionStatus> statusSlots)
    {
        var blueAlive = statusSlots.GetTeam(Team.Blue).Where(s => !s.IsDead);
        var redAlive = statusSlots.GetTeam(Team.Red).Where(s => !s.IsDead);

        int blueScore = blueAlive.Sum(s => s.Stat.Attack + s.Stat.Defense);
        int redScore = redAlive.Sum(s => s.Stat.Attack + s.Stat.Defense);

        Team winner = blueScore == redScore ? Team.All : (blueScore > redScore ? Team.Blue : Team.Red);

        var blueInfo = new TeamScoreInfo(blueAlive.Sum(s => s.Stat.Attack), blueAlive.Sum(s => s.Stat.Defense), 0, 0, 0);
        var redInfo = new TeamScoreInfo(redAlive.Sum(s => s.Stat.Attack), redAlive.Sum(s => s.Stat.Defense), 0, 0, 0);

        return new MatchResult(blueInfo, redInfo, winner);
    }
}
