using UnityEngine.SceneManagement;
using Match;

public class BattleInintialzer : ISceneLoader
{
    const string BattleSceneName = "Battle";

    public void LoadBattleScene(MatchData match)
    {
        var playerDatas = new PlayerMatchData(new PlayerData(1, "@@", new JsonMasterySaver().Load().BoardCollection), new PlayerData(2, "AI", new JsonMasterySaver().Load().BoardCollection));
        MatchContext.MatchInit(playerDatas, 2, ChampionDataLoder.AllId);
        SceneManager.LoadScene(BattleSceneName);
    }
}