using System.Linq;

public class ScoreConvertor
{
    public GameScoreInfo Convert(SlotStorage<ChampionStatus> storage)
    {
        ScoreInfo blueScore = CalculateTeamScore(storage, Team.Blue);
        ScoreInfo redScore = CalculateTeamScore(storage, Team.Red);

        return new GameScoreInfo(blueScore, redScore);
    }

    ScoreInfo CalculateTeamScore(SlotStorage<ChampionStatus> storage, Team team)
    {
        var stats = storage.GetTeam(team).Select(x => x.Stat);
        int totalAtt = ScoreCalculator.CalculateAttack(stats);
        int totalDef = ScoreCalculator.CalculateDefense(stats);
        int totalSpd = ScoreCalculator.CalculateSpeed(stats);

        return new ScoreInfo(totalAtt, totalDef, totalSpd);
    }
}