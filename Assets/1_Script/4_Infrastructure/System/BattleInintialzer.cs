using Match;
using UnityEngine.SceneManagement;

public class BattleInintialzer : IBattleResolver
{
    const string BattleSceneName = "Battle";
    public void LoadBattleScene() => SceneManager.LoadScene(BattleSceneName);

    public void Resolve(MatchData match)
    {
        MatchContext.MatchInit(match, 2, ChampionDataLoder.AllId);
        SceneManager.LoadScene(BattleSceneName);
    }
}