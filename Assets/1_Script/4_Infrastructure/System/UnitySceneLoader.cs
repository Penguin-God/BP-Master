using UnityEngine.SceneManagement;

public class UnitySceneLoader : ISceneLoader
{
    const string BattleSceneName = "Battle";

    public void LoadBattleScene(MatchData match)
    {
        GameContext.SetupMatch(match);
        SceneManager.LoadScene(BattleSceneName);
    }
}

public static class GameContext
{
    public static MatchData CurrentMatch { get; set; }

    public static void SetupMatch(MatchData match)
    {
        CurrentMatch = match;
    }
}