using Match;
using UnityEngine.SceneManagement;

public class BattleInitializer : IBattleResolver
{
    const string BattleSceneName = "Battle";
    readonly int WinCount;
    public BattleInitializer(int winCount) => WinCount = winCount;

    public void Resolve(MatchData match)
    {
        MatchContext.MatchInit(match, WinCount, ChampionDataLoder.AllId);
        SceneManager.LoadScene(BattleSceneName);
    }
}