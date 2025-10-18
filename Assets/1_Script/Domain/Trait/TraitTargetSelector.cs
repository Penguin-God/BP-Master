using System.Collections.Generic;
using System.Linq;

public class TraitTargetSelector
{
    readonly int TeamSize;
    readonly TraitTargetRule Rule;

    readonly HashSet<SlotData> selected = new HashSet<SlotData>();
    public IEnumerable<SlotData> Targets => selected;

    public TraitTargetSelector(int teamSize, TraitTargetRule rule)
    {
        TeamSize = teamSize;
        Rule = rule;
    }

    public bool IsFull
    {
        get
        {
            if (Rule.TargetRange == TargetRange.All) return selected.Count > 0;
            else return selected.Count >= CapacityFor(Rule.TargetRange);
        }
    }

    public bool CanSelected(SlotData clicked) => selected.Contains(clicked) == false && IsFull == false;
    public void Select(SlotData target)
    {
        if(CanSelected(target) == false) return;

        if (Rule.TargetRange == TargetRange.All) SelectAll(target);
        else selected.Add(target);
    }

    void SelectAll(SlotData target)
    {
        selected.Clear();

        if (Rule.TargetSide == Side.All) SelectAdds(AllTeamSlots);
        else SelectAdds(TeamSlots(target.Team));
    }

    void SelectAdds(IEnumerable<SlotData> slots)
    {
        foreach (var s in slots)
            selected.Add(s);
    }

    int CapacityFor(TargetRange range) => range switch
    {
        TargetRange.Single => 1,
        TargetRange.Double => 2,
        TargetRange.Triple => 3,
        _ => 0
    };

    IEnumerable<SlotData> TeamSlots(Team team) => Enumerable.Range(0, TeamSize).Select(i => new SlotData(team, i));
    IEnumerable<SlotData> AllTeamSlots => TeamSlots(Team.Blue).Concat(TeamSlots(Team.Red));
}