using UnityEngine.SceneManagement;

public enum SceneType
{
    Lobby,
    Battle,
    Swap,
}

public static class SceneLoadHelper
{
    public static void LoadScene(SceneType sceneType) => SceneManager.LoadScene(sceneType.ToString());
}