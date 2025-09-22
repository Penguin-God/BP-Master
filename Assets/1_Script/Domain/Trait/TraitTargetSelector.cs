using System.Collections.Generic;
using System.Linq;

public struct SlotData
{
    public readonly Team Team;
    public readonly int Index;

    public SlotData(Team team, int index)
    {
        Team = team;
        Index = index;
    }
}

public class TraitTargetSelector
{
    readonly int teamCount;
    public TraitTargetSelector(int count) => teamCount = count;
    public IEnumerable<SlotData> GetTargetableSlot(Team team, Side side)
    {
        Team targetTeam = BanPickEnumCaster.GetTargetTeam(team, side);
        return Enumerable.Range(0, teamCount).Select(i => new SlotData(targetTeam, i));
    }
    public IEnumerable<SlotData> GetTargetSlots(TargetRange targetRange, SlotData targetSlot)
    {
        switch (targetRange)
        {
            case TargetRange.Single:return new[] { targetSlot };
            case TargetRange.All: return Enumerable.Range(0, teamCount).Select(i => new SlotData(targetSlot.Team, i));
            default: return null;
        }
    }
}
