using System.Collections.Generic;
using System.Linq;

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

        if (context.Rule.TargetRange == TargetRange.All) SelectAll(target);
        else selected.Add(target);
    }

    void SelectAll(SlotData target)
    {
        selected.Clear();

        if (context.Rule.TargetSide == Side.All) selected.UnionWith(AllTeamSlots);
        else selected.UnionWith(TeamSlots(target.Team));
    }

    IEnumerable<SlotData> TeamSlots(Team team) => Enumerable.Range(0, context.TeamCount.GetTeamCount(team)).Select(i => new SlotData(team, i));
    IEnumerable<SlotData> AllTeamSlots => TeamSlots(Team.Blue).Concat(TeamSlots(Team.Red));
}