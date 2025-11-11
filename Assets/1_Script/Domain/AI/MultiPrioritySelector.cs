using System.Collections.Generic;
using System.Linq;

public sealed class MultiPrioritySelector : IBanSelector, IPickSelector
{
    readonly PrioritySelector[] selectors;
    readonly MasteryManager mastery;

    public MultiPrioritySelector(MasteryManager mastery, IEnumerable<PrioritySelector> selectors)
    {
        this.mastery = mastery;
        this.selectors = selectors.ToArray();
    }

    PrioritySelector ChooseByMasterySum(HashSet<int> selectableIds)
    {
        PrioritySelector best = selectors[0];
        int bestSum = SumSelectableMastery(best, selectableIds);

        for (int i = 1; i < selectors.Length; i++)
        {
            var s = selectors[i];
            int sum = SumSelectableMastery(s, selectableIds);
            if (sum > bestSum)
            {
                best = s;
                bestSum = sum;
            }
        }

        return best; // 동률이면 앞선 셀렉터 유지
    }

    int SumSelectableMastery(PrioritySelector selector, HashSet<int> selectableIds)
    {
        int total = 0;
        foreach (var id in selector.PickPlan)
            if (selectableIds.Contains(id))
                total += mastery.GetMastery(id);
        return total;
    }

    public int Pick(HashSet<int> selectableIds)
        => ChooseByMasterySum(selectableIds).Pick(selectableIds);

    public int Ban(HashSet<int> selectableIds)
        => ChooseByMasterySum(selectableIds).Ban(selectableIds);
}
