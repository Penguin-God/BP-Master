using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SocialPlatforms;

public class SkillSlotFilter
{
    readonly int TeamSize;
    SlotStorage<bool> triatUseFlagSlots;
    public SkillSlotFilter(SlotStorage<bool> triatUseFlagSlots)
    {
        TeamSize = triatUseFlagSlots.GetTeam(Team.Blue).Count();
        this.triatUseFlagSlots = triatUseFlagSlots;
    }

    public IEnumerable<SlotData> FilteringUseableSlots(Team team)
        => Enumerable.Range(0, TeamSize)
                 .Select(i => new SlotData(team, i))
                 .Where(slot => triatUseFlagSlots.GetSlot(slot) == false);

    public IEnumerable<SlotData> FilteringTargetSlots(Team team, IEnumerable<Side> sides)
    {
        var side = EnumCaster.MergeSide(sides);
        return new SkillTargetFinder(TeamSize).GetTargetableSlot(team, side);
    }
}

public class SkillTargetCounter
{
    readonly int BlueCount;
    readonly int RedCount;

    public SkillTargetCounter(int blueCount, int redCount)
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
    readonly SkillTargetCounter TeamCounter;
    public SkillTargetFilter(SkillTargetCounter teamCounter) => TeamCounter = teamCounter;

    IEnumerable<SlotData> GetTargetableSlot(Team team, Side side)
    {
        Team targetTeam = EnumCaster.GetTargetTeam(team, side);

        if (targetTeam == Team.All) return GetAllSlots();
        else return GetTeamSlots(targetTeam);
    }

    IEnumerable<SlotData> GetTeamSlots(Team team) => Enumerable.Range(0, TeamCounter.GetTeamCount(team)).Select(i => new SlotData(team, i));
    IEnumerable<SlotData> GetAllSlots() => new[] { Team.Blue, Team.Red }.SelectMany(x => GetTeamSlots(x));

    public IEnumerable<SlotData> FilteringTargetSlots(Team team, IEnumerable<Side> sides)
    {
        var side = EnumCaster.MergeSide(sides);
        return GetTargetableSlot(team, side);
    }
}
