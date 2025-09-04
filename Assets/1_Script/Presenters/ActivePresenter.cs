using System.Collections.Generic;

public class ActivePresenter
{
    int currentTargetCount;
    HashSet<int> targets = new();

    public bool AddTarget(int target)
    {
        if (targets.Count >= currentTargetCount) return false;
        return targets.Add(target);
    }

    public void SelectTrait(Trait trait, int targetCount)
    {
        currentTargetCount = targetCount;
    }
}
