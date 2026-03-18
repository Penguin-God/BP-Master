using UnityEngine.SceneManagement;

public class BattleInintialzer : ISceneLoader
{
    const string BattleSceneName = "Battle";
    public void LoadBattleScene() => SceneManager.LoadScene(BattleSceneName);
}