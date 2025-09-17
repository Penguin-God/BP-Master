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

    public IEnumerable<SlotData> GetTargetableSlot(Team team, Side side) => Enumerable.Range(0, teamCount).Select(i => new SlotData(BanPickEnumCaster.GetTargetTeam(team, side), i));

    public IEnumerable<SlotData> GetTargetSlots(Team team, Side targetSide, TargetRange targetRange, SlotData targetSlot)
    {
        var targetTeam = BanPickEnumCaster.GetTargetTeam(team, targetSide);
        if (targetSlot.Team != targetTeam)
        {
            UnityEngine.Debug.Log($"잘못된 팀 {targetTeam}");
            return null;
        }

        switch (targetRange)
        {
            case TargetRange.Single:return new[] { new SlotData(targetTeam, targetSlot.Index) };
            case TargetRange.All: return Enumerable.Range(0, teamCount).Select(i => new SlotData(targetTeam, i));
            default: return null;
        }
    }
}
