using UnityEngine.SceneManagement;
using Match;

public class BattleInintialzer : ISceneLoader
{
    const string BattleSceneName = "Battle";

    public void LoadBattleScene(MatchData match)
    {
        MatchContext.MatchInit(match, 2, new int[] { 1 }, ChampionDataLoder.AllId);
        SceneManager.LoadScene(BattleSceneName);
    }
}