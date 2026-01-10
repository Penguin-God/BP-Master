using System.Linq;

public record ScoreInfo(int Att, int Def, int Speed);
public record GameScoreInfo(ScoreInfo Blue, ScoreInfo Red);

public static class ScoreDeltaCalculator
{
    public static GameScoreInfo CalculateStatDelta(SlotStorage<ChampionStatus> origin, SlotStorage<ChampionStatus> after)
    {
        ScoreInfo blueDelta = CalculateTeamDelta(origin, after, Team.Blue);
        ScoreInfo redDelta = CalculateTeamDelta(origin, after, Team.Red);

        return new GameScoreInfo(blueDelta, redDelta);
    }

    static ScoreInfo CalculateTeamDelta(SlotStorage<ChampionStatus> origin, SlotStorage<ChampionStatus> after, Team team)
    {
        int totalAtt = 0;
        int totalDef = 0;
        int totalSpd = 0;

        var teamSlots = origin.GetAllSlotDatas().Where(s => s.Team == team);

        foreach (var slotData in teamSlots)
        {
            ChampionStatus beforeStatus = origin.GetSlot(slotData);
            ChampionStatus afterStatus = after.GetSlot(slotData);

            totalAtt += afterStatus.Stat.Attack - beforeStatus.Stat.Attack;
            totalDef += afterStatus.Stat.Defense - beforeStatus.Stat.Defense;
            totalSpd += afterStatus.Stat.Speed - beforeStatus.Stat.Speed;
        }

        return new ScoreInfo(totalAtt, totalDef, totalSpd);
    }
}