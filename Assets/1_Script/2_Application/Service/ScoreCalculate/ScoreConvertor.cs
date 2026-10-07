using System.Linq;

public static class ScoreConvertor
{
    public static GameScoreInfo Convert(SlotStorage<ChampionStatus> storage)
    {
        ScoreInfo blueScore = CalculateTeamScore(storage, Team.Blue);
        ScoreInfo redScore = CalculateTeamScore(storage, Team.Red);

        return new GameScoreInfo(blueScore, redScore);
    }

    static ScoreInfo CalculateTeamScore(SlotStorage<ChampionStatus> storage, Team team)
    {
        var stats = storage.GetTeam(team).Select(x => x.Stat);
        int totalAtt = ScoreCalculator.CalculateAttack(stats);
        int totalDef = ScoreCalculator.CalculateDefense(stats);
        int totalSpd = ScoreCalculator.CalculateSpeed(stats);

        return new ScoreInfo(totalAtt, totalDef, totalSpd);
    }
}