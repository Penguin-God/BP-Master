using System;
using System.Collections.Generic;
using System.Linq;

public class TargetCountCalculator
{
    readonly int BlueCount;
    readonly int RedCount;

    public TargetCountCalculator(int blueCount, int redCount)
    {
        BlueCount = blueCount;
        RedCount = redCount;
    }

    public int GetTeamCount(Team team) => team == Team.Blue ? BlueCount : RedCount;

    public int CalculateTargetCount(Team team, SkillTargetRule rule)
    {
        if (rule.TargetRange == TargetRange.All)
        {
            if (rule.TargetSide == Side.All) return BlueCount + RedCount;
            else return GetTeamCount(EnumCaster.GetTargetTeam(team, rule.TargetSide));
        }

        return GetTargetableCount(team, rule);
    }

    int GetTargetableCount(Team team, SkillTargetRule rule) => Math.Min(GetTeamCount(EnumCaster.GetTargetTeam(team, rule.TargetSide)), CalculateFixedCount(rule.TargetRange));

    int CalculateFixedCount(TargetRange targetRange) => targetRange switch
    {
        TargetRange.Single => 1,
        TargetRange.Double => 2,
        TargetRange.Triple => 3,
        _ => 0
    };
}

public class SkillTargetFilter
{
    readonly TargetCountCalculator TeamCounter;
    public SkillTargetFilter(TargetCountCalculator teamCounter) => TeamCounter = teamCounter;

    public IEnumerable<SlotData> FilteringTargetSlots(Team team, IEnumerable<Side> sides)
    {
        var side = EnumCaster.MergeSide(sides);
        Team targetTeam = EnumCaster.GetTargetTeam(team, side);

        if (targetTeam == Team.All) return GetAllSlots();
        else return GetTeamSlots(targetTeam);
    }

    IEnumerable<SlotData> GetTeamSlots(Team team) => Enumerable.Range(0, TeamCounter.GetTeamCount(team)).Select(i => new SlotData(team, i));
    IEnumerable<SlotData> GetAllSlots() => new[] { Team.Blue, Team.Red }.SelectMany(x => GetTeamSlots(x));
}
