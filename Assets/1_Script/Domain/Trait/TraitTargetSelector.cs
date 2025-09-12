using System;
using System.Collections.Generic;
using System.Linq;

public struct ChampionSlot
{
    public readonly Team Team;
    public readonly int Index;

    public ChampionSlot(Team team, int index)
    {
        Team = team;
        Index = index;
    }
}

public class TraitTargetSelector
{
    readonly int teamCount;
    public TraitTargetSelector(int count) => teamCount = count;

    public IEnumerable<int> GetTargetIds(TargetRange range, int targetIndex)
    {
        switch (range)
        {
            case TargetRange.Single: return new int[] { targetIndex };
            case TargetRange.All: return Enumerable.Range(0, teamCount);
            default: return null;
        }
    }

    public IEnumerable<ChampionSlot> GetTargetableSlot(Team team, Side side) => Enumerable.Range(0, teamCount).Select(i => new ChampionSlot(BanPickEnumCaster.GetTargetTeam(team, side), i));

    public IEnumerable<ChampionSlot> GetTargetSlots(Team team, Side targetSide, TargetRange targetRange, ChampionSlot targetSlot)
    {
        var targetTeam = BanPickEnumCaster.GetTargetTeam(team, targetSide);
        if (targetSlot.Team != targetTeam) return null;

        switch (targetRange)
        {
            case TargetRange.Single:return new[] { new ChampionSlot(targetTeam, targetSlot.Index) };
            case TargetRange.All: return Enumerable.Range(0, teamCount).Select(i => new ChampionSlot(targetTeam, i));
            default: return null;
        }
    }
}
