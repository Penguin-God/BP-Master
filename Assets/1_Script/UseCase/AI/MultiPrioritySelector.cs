using System.Collections.Generic;
using System.Linq;

public sealed class MultiPrioritySelector : IBanSelector, IPickSelector
{
    readonly IEnumerable<PrioritySelector> selectors;
    readonly MasteryCollection mastery;

    public MultiPrioritySelector(MasteryCollection mastery, IEnumerable<PrioritySelector> selectors)
    {
        this.mastery = mastery;
        this.selectors = selectors.ToArray();
    }

    PrioritySelector SelectBuild()
        => selectors
            .OrderByDescending(x => SumPickMastery(x))
            .FirstOrDefault();

    int SumPickMastery(PrioritySelector selector) => selector.PickPlan.Sum(id => mastery.GetMastery(id));

    public int Pick(HashSet<int> selectableIds) => SelectBuild().Pick(selectableIds);
    public int Ban(HashSet<int> selectableIds) => SelectBuild().Ban(selectableIds);
}
