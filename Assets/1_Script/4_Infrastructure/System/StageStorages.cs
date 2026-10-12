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

    public static void ShowStageReward(UI_StageRewardDisplayer displayerPrefab, int unlockStage)
    {
        if (displayerPrefab == null) return;
        if (ShouldShowStageReward(unlockStage) == false) return;

        var rewardCards = CardListLoder.LoadCardListSO(unlockStage);
        if (rewardCards == null) return;

        Object.Instantiate(displayerPrefab).Draw(rewardCards.CardList);

        PlayerPrefs.SetInt(StageProgressKey, unlockStage);
        PlayerPrefs.Save();
    }

    static bool ShouldShowStageReward(int unlockStage)
    {
        int seenStage = PlayerPrefs.GetInt(StageProgressKey, defaultValue: 0);
        return unlockStage > seenStage;
    }
}