using UnityEngine;

public class PlayerPrefsTutorialStorage : ITutorialStorage
{
    public bool HasSeen(TutorialType type) => PlayerPrefs.GetInt($"Tutorial_{type}", 0) == 1;

    public void MarkAsSeen(TutorialType type)
    {
        // PlayerPrefs.SetInt($"Tutorial_{type}", 1);
        // PlayerPrefs.Save();
    }
}