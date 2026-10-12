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

public static class StageRewardPresenter
{
    const string StageProgressKey = "RewardSeenStage";

    public static bool ShouldShowStageReward(int unlockStage)
    {
        int seenStage = PlayerPrefs.GetInt(StageProgressKey, defaultValue: 0);
        
        return unlockStage > seenStage;
    }
}