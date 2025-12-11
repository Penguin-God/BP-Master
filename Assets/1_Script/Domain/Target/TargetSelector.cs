using System.Collections.Generic;
using System.Linq;

public class TraitTargetSelector
{
    readonly HashSet<SlotData> selected = new HashSet<SlotData>();
    public IEnumerable<SlotData> Targets => selected;

    readonly int TeamSize;
    readonly SkillTargetRule Rule;
    readonly TargetCounter targetCounter;
    public TraitTargetSelector(int teamSize, SkillTargetRule rule)
    {
        TeamSize = teamSize;
        targetCounter = new TargetCounter(teamSize);
        Rule = rule;
    }

    public bool IsFull
    {
        get
        {
            if (Rule.TargetRange == TargetRange.All) return selected.Count > 0;
            else return selected.Count >= targetCounter.CalculateTargetCount(Rule);
        }
    }

    public bool CanSelected(SlotData target) => selected.Contains(target) == false && IsFull == false;
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

    IEnumerable<SlotData> TeamSlots(Team team) => Enumerable.Range(0, TeamSize).Select(i => new SlotData(team, i));
    IEnumerable<SlotData> AllTeamSlots => TeamSlots(Team.Blue).Concat(TeamSlots(Team.Red));
}

public class SkillTargetSelector
{
    readonly HashSet<SlotData> selected = new HashSet<SlotData>();
    public IEnumerable<SlotData> Targets => selected;

    readonly Team Team;
    readonly SkillTargetRule Rule;
    readonly SkillTargetCounter targetCounter;
    public SkillTargetSelector(Team team, SkillTargetCounter skillTargetCounter, SkillTargetRule rule)
    {
        Team = team;
        targetCounter = skillTargetCounter;
        Rule = rule;
    }

    public bool IsFull => selected.Count >= targetCounter.CalculateTargetCount(Team, Rule);

    bool CanSelected(SlotData target) => selected.Contains(target) == false && IsFull == false;
    public void Select(SlotData target)
    {
        if (CanSelected(target) == false) return;

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

    IEnumerable<SlotData> TeamSlots(Team team) => Enumerable.Range(0, targetCounter.GetTeamCount(team)).Select(i => new SlotData(team, i));
    IEnumerable<SlotData> AllTeamSlots => TeamSlots(Team.Blue).Concat(TeamSlots(Team.Red));
}