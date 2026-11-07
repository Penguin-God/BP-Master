using System.Collections.Generic;
using System.Linq;

public static class StageUnlockChecker
{
    public static bool IsUnlocked(int stageIndex, int clearedStage) => stageIndex <= clearedStage;

    public static List<bool> GetUnlockStates(int stageCount, int clearedStage) => Enumerable.Range(0, stageCount).Select(i => IsUnlocked(i, clearedStage)).ToList();
}
