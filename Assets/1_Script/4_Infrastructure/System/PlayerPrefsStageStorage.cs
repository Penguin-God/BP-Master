using UnityEngine;


public class PlayerPrefsStageStorage : IStageStorage
{
    const string StageProgressKey = "AI_UnlockedStage";

    public int LoadUnlockedStage() => PlayerPrefs.GetInt(StageProgressKey, defaultValue: 0);

    public void SaveUnlockedStage(int stageIndex)
    {
        PlayerPrefs.SetInt(StageProgressKey, stageIndex);
        PlayerPrefs.Save();
    }
}