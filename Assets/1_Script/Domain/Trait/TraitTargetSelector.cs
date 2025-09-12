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

    public IEnumerable<ChampionSlot> GetTargetSlot(Team team, Side side, TargetRange range, ChampionSlot clickedSlot)
    {
        var expectedTeam = BanPickEnumCaster.GetTargetTeam(team, side);
        if (clickedSlot.Team != expectedTeam) return null;

        switch (range)
        {
            case TargetRange.Single: return new[] { new ChampionSlot(expectedTeam, clickedSlot.Index) };
            case TargetRange.All: return Enumerable.Range(0, teamCount).Select(i => new ChampionSlot(expectedTeam, i));
            default: return null;
        }
    }
}
