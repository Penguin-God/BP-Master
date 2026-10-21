using System.Collections.Generic;
using System.Linq;

public record TargetSelection(SlotData Caster, TargetFindContext Context, HashSet<SlotData> Selected)
{
    public static TargetSelection Create(SlotData caster, TargetFindContext context) => new(caster, context, new HashSet<SlotData>());
    public bool IsFull => Selected.Count >= TargetCountCalculator.CalculateTargetCount(Context);
    public IEnumerable<SlotData> GetSelectableTargets() => TargetCountCalculator.CreateTargetingableSlots(Caster, Context.Rule.TargetSide, Context.TeamCount).Except(Selected);
}

public static class TargetSelectionRules
{
    public static TargetSelection Select(TargetSelection current, SlotData target)
    {
        if (current.IsFull || current.Selected.Contains(target)) return current;

        var nextSelected = new HashSet<SlotData>(current.Selected);

        if (current.Context.Rule.TargetRange == TargetRange.All)
        {
            var allSlots = current.Context.Rule.TargetSide == Side.All ? GetTarget(Side.All) : GetTarget(Side.Self);
            return current with { Selected = allSlots.ToHashSet() };
        }
        else
        {
            nextSelected.Add(target);
            return current with { Selected = nextSelected };
        }

        IEnumerable<SlotData> GetTarget(Side side) => TargetCountCalculator.CreateTargetingableSlots(target, side, current.Context.TeamCount);
    }
}