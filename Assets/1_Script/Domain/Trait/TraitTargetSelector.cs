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

    public override string ToString() => $"SlotData => Team :{Team}, index : {Index}";
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
            case TargetRange.All: return GetTeamSlots(targetSlot.Team);
            default: return null;
        }
    }

    public IEnumerable<SlotData> GetTargetSlots(TraitTargetRule rule, SlotData targetSlot)
    {
        if (rule.TargetSide == Side.All) return GetAllSlots();

        switch (rule.TargetRange)
        {
            case TargetRange.Single: return new[] { targetSlot };
            case TargetRange.All: return GetTeamSlots(targetSlot.Team);
            default: throw new System.Exception($"정의 되지 않은 규칙 조합 {rule.TargetSide} : {rule.TargetRange}");
        }
    }

    IEnumerable<SlotData> GetTeamSlots(Team team) => Enumerable.Range(0, teamCount).Select(i => new SlotData(team, i));
    IEnumerable<SlotData> GetAllSlots() => new[] { Team.Blue, Team.Red }.SelectMany(x => GetTeamSlots(x));
}
