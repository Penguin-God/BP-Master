
public class ChampionStatValueCalculator
{
    readonly int SPPED_VALUE;

    public ChampionStatValueCalculator(int speedValue) => SPPED_VALUE = speedValue;

    public int CalcualteTeamStatValue(GameScoreInfo data, Team myTeam)
    {
        int blueScore = CalculateRawValue(data.Blue);
        int redScore = CalculateRawValue(data.Red);

        if (myTeam == Team.Blue) return blueScore - redScore;
        else return redScore - blueScore;
    }

    int CalculateRawValue(ScoreInfo info) => info.Att + info.Def + (info.Speed * SPPED_VALUE);
}