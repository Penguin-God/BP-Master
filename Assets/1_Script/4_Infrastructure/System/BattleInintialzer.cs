using Match;

public class BattleInitializer : IBattleResolver
{
    readonly int WinCount;
    public BattleInitializer(int winCount) => WinCount = winCount;

    public void Resolve(MatchData match)
    {
        MatchContext.MatchInit(match, WinCount, ChampionDataLoder.AllId);
        SceneLoadHelper.LoadScene(SceneType.Battle);
    }
}