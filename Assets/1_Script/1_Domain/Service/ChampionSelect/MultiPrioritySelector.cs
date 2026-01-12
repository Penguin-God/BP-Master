using System.Collections.Generic;
using System.Linq;

public sealed class MultiPrioritySelector : IChampionSelector
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

    int SumPickMastery(PrioritySelector selector) => selector.Plan.Sum(id => mastery.GetMasteryLevel(id));

    public int Select(HashSet<int> selectableIds) => SelectBuild().Select(selectableIds);
}
