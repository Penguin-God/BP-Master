using System;
using System.Collections.Generic;
using System.Linq;

public record TeamCount(int Blue, int Red)
{
    public int Sum => Blue + Red;
    public int GetTeamCount(Team team) => team == Team.Blue ? Blue : Red;
}

public record TargetFindContext(Team Team, SkillTargetRule Rule, TeamCount TeamCount);

public static class TargetCountCalculator
{
    public static int CalculateTargetCount(TargetFindContext context)
    {
        if (context.Rule.TargetRange == TargetRange.All)
        {
            if (context.Rule.TargetSide == Side.All) return context.TeamCount.Sum;
            else return GetTargetTeamCount();
        }

        return Math.Min(GetTargetTeamCount(), CalculateFixedCount(context.Rule.TargetRange));

        int GetTargetTeamCount() => context.TeamCount.GetTeamCount(EnumCaster.GetTargetTeam(context.Team, context.Rule.TargetSide));

        int CalculateFixedCount(TargetRange targetRange) => targetRange switch
        {
            TargetRange.Single => 1,
            TargetRange.Double => 2,
            TargetRange.Triple => 3,
            _ => 0
        };
    }

    public static IEnumerable<SlotData> CreateTargetingableSlots(SlotData oneSelf, Side side, TeamCount teamCount)
    {
        Team targetTeam = EnumCaster.GetTargetTeam(oneSelf.Team, side);
        UnityEngine.Debug.Log(targetTeam);
        UnityEngine.Debug.Log(side);
        if (targetTeam == Team.All) return CreateAllSlots(teamCount);
        else if(side == Side.OneSelf) return new[] { oneSelf };
        else return CreateTeamSlots(targetTeam, teamCount);


        static IEnumerable<SlotData> CreateAllSlots(TeamCount teamCount) => new[] { Team.Blue, Team.Red }.SelectMany(t => CreateTeamSlots(t, teamCount));
        static IEnumerable<SlotData> CreateTeamSlots(Team team, TeamCount teamCount) => Enumerable.Range(0, teamCount.GetTeamCount(team)).Select(i => new SlotData(team, i));
    }

    public static IEnumerable<T> FilterTargets<T>(SlotStorage<T> storage, Team team, IEnumerable<Side> sides)
    {
        var side = EnumCaster.MergeSide(sides);
        Team targetTeam = EnumCaster.GetTargetTeam(team, side);

        if (targetTeam == Team.All) return storage.GetAll();
        return storage.GetTeam(targetTeam);
    }
}