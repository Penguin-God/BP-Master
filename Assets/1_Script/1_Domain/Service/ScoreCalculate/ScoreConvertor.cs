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
        int totalAtt = 0;
        int totalDef = 0;
        int totalSpd = 0;

        foreach (var slotData in storage.GetAllSlotDatas().Where(s => s.Team == team))
        {
            var status = storage.GetSlot(slotData);
            if (status != null)
            {
                totalAtt += status.Stat.Attack;
                totalDef += status.Stat.Defense;
                totalSpd += status.Stat.Speed;
            }
        }

        return new ScoreInfo(totalAtt, totalDef, totalSpd);
    }
}