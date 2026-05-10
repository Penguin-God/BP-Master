using UnityEngine;

public enum TutorialType
{
    GameStart,
    Mastery,
    BattleIntro,
    FearlessDraft
}

public interface ITutorialStorage
{
    bool HasSeen(TutorialType type);
    void SaveTutorialSeen(TutorialType type);
}

public class PlayerPrefsTutorialStorage : ITutorialStorage
{
    public bool HasSeen(TutorialType type) => PlayerPrefs.GetInt(type.ToString(), 0) == 1;

    public void SaveTutorialSeen(TutorialType type)
    {
        PlayerPrefs.SetInt(type.ToString(), 1);
        PlayerPrefs.Save();
    }
}