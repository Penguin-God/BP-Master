using System.Collections.Generic;

public class ActivePresenter
{
    readonly int MaxTargetCount;
    int currentTargetCount;
    HashSet<int> targets = new();

    public ActivePresenter(int maxCount) => MaxTargetCount = maxCount;

    public bool AddTarget(int target)
    {
        if (targets.Count >= currentTargetCount) return false;
        return targets.Add(target);
    }

    public void SelectTrait(Trait trait, int targetCount)
    {
        UpdateTarget(targetCount);
    }

    void UpdateTarget(int value)
    {
        int newTargetCount = value > MaxTargetCount ? MaxTargetCount : value;
        currentTargetCount = newTargetCount;
    }
}
