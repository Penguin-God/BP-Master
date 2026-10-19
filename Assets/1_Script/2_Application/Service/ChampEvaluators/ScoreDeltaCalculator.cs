using System.Linq;

public record ScoreInfo(int Att, int Def, int Speed)
{
    public static ScoreInfo operator +(ScoreInfo a, ScoreInfo b) => new(a.Att + b.Att, a.Def + b.Def, a.Speed + b.Speed);
    public static ScoreInfo operator -(ScoreInfo a, ScoreInfo b) => new(a.Att - b.Att, a.Def - b.Def, a.Speed - b.Speed);
}

public record GameScoreInfo(ScoreInfo Blue, ScoreInfo Red)
{
    public static GameScoreInfo operator +(GameScoreInfo a, GameScoreInfo b)=> new(a.Blue + b.Blue, a.Red + b.Red);
    public static GameScoreInfo operator -(GameScoreInfo a, GameScoreInfo b) => new(a.Blue - b.Blue, a.Red - b.Red);
}

public static class ScoreDeltaCalculator
{
    public static int CalculateScoreDelta(GameScoreInfo data, Team myTeam, int speedValue)
    {
        int blueScore = CalculateRawValue(data.Blue);
        int redScore = CalculateRawValue(data.Red);

        if (myTeam == Team.Blue) return blueScore - redScore;
        else return redScore - blueScore;

        int CalculateRawValue(ScoreInfo info) => info.Att + info.Def + (info.Speed * speedValue);
    }

    public static GameScoreInfo CalculateStatDelta(SlotStorage<ChampionStatus> origin, SlotStorage<ChampionStatus> after)
    {
        ScoreInfo blueDelta = CalculateTeamDelta(origin, after, Team.Blue);
        ScoreInfo redDelta = CalculateTeamDelta(origin, after, Team.Red);

        return new GameScoreInfo(blueDelta, redDelta);
    }

    static ScoreInfo CalculateTeamDelta(SlotStorage<ChampionStatus> origin, SlotStorage<ChampionStatus> after, Team team)
    {
        int totalAtt = after.GetTeam(team).Sum(x => x.Stat.Attack) - origin.GetTeam(team).Sum(x => x.Stat.Attack);
        int totalDef = after.GetTeam(team).Sum(x => x.Stat.Defense) - origin.GetTeam(team).Sum(x => x.Stat.Defense);
        int totalSpd = after.GetTeam(team).Sum(x => x.Stat.Speed) - origin.GetTeam(team).Sum(x => x.Stat.Speed);

        return new ScoreInfo(totalAtt, totalDef, totalSpd);
    }
}