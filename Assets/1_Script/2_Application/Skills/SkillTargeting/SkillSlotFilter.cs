using System;
using System.Collections.Generic;
using System.Linq;

public record TeamCount(int Blue, int Red)
{
    public int Sum => Blue + Red;
    public int GetTeamCount(Team team) => team == Team.Blue ? Blue : Red;
}

public record TargetFindContext(Team Team, SkillTargetRule Rule, TeamCount TeamCount);

public readonly struct TargetCountCalculator
{
    readonly int BlueCount;
    readonly int RedCount;

    public TargetCountCalculator(int blueCount, int redCount)
    {
        BlueCount = blueCount;
        RedCount = redCount;
    }

    public TeamCount TeamCount => new TeamCount(BlueCount, RedCount);

    public IEnumerable<SlotData> GetTargets(Team casterTeam, Skill skill, ISkillTargetSelector selector)
    {
        int count = CalculateTargetCount(casterTeam, EnumCaster.MergeRule(skill.Rules));
        var candidates = FilteringTargetSlots(casterTeam, skill.Sides);
        return selector.SelectTargets(candidates, count, skill);
    }

    public int GetTeamCount(Team team) => team == Team.Blue ? BlueCount : RedCount;

    public int CalculateTargetCount(Team team, SkillTargetRule rule) => CalculateTargetCount(new TargetFindContext(team, rule, TeamCount));

    public static int CalculateTargetCount(TargetFindContext context)
    {
        if (context.Rule.TargetRange == TargetRange.All)
        {
            if (context.Rule.TargetSide == Side.All) return context.TeamCount.Sum;
            else return GetTargetTeamCount();
        }

        return Math.Min(GetTargetTeamCount(), CalculateFixedCount(context.Rule.TargetRange));

        int GetTargetTeamCount() => context.TeamCount.GetTeamCount(EnumCaster.GetTargetTeam(context.Team, context.Rule.TargetSide));
    }

    static int CalculateFixedCount(TargetRange targetRange) => targetRange switch
    {
        TargetRange.Single => 1,
        TargetRange.Double => 2,
        TargetRange.Triple => 3,
        _ => 0
    };

    public IEnumerable<SlotData> FilteringTargetSlots(Team team, IEnumerable<Side> sides)
    {
        var side = EnumCaster.MergeSide(sides);
        Team targetTeam = EnumCaster.GetTargetTeam(team, side);

        if (targetTeam == Team.All) return GetAllSlots();
        else return GetTeamSlots(targetTeam);
    }

    IEnumerable<SlotData> GetAllSlots() => new[] { Team.Blue, Team.Red }.SelectMany(GetTeamSlots);
    IEnumerable<SlotData> GetTeamSlots(Team team) => Enumerable.Range(0, GetTeamCount(team)).Select(i => new SlotData(team, i));
}