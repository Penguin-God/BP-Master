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

public class SkillTargetSelector
{
    readonly HashSet<SlotData> selected = new HashSet<SlotData>();
    public IEnumerable<SlotData> Targets => selected;

    readonly TargetFindContext context;
    public SkillTargetSelector(TargetFindContext context) => this.context = context;

    public bool IsFull => selected.Count >= TargetCountCalculator.CalculateTargetCount(context);

    bool CanSelected(SlotData target) => selected.Contains(target) == false && IsFull == false;
    public void Select(SlotData target)
    {
        if (CanSelected(target) == false) return;

        if (context.Rule.TargetRange == TargetRange.All)
        {
            selected.Clear();
            selected.UnionWith(SelectAll(target.Team));
        }
        else selected.Add(target);
    }

    IEnumerable<SlotData> SelectAll(Team team)
    {
        if (context.Rule.TargetSide == Side.All) return AllTeamSlots;
        return TeamSlots(team);
    }

    IEnumerable<SlotData> TeamSlots(Team team) => Enumerable.Range(0, context.TeamCount.GetTeamCount(team)).Select(i => new SlotData(team, i));
    IEnumerable<SlotData> AllTeamSlots => TeamSlots(Team.Blue).Concat(TeamSlots(Team.Red));

    public IEnumerable<SlotData> GetSelectableTarget() => TargetCountCalculator.CreateTargetingableSlots(context.Team, context.Rule.TargetSide, context.TeamCount).Except(Targets);
}