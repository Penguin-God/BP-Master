using System.Collections.Generic;
using System.Linq;

public class TraitTargetFinder
{
    readonly int teamCount;
    public TraitTargetFinder(int count) => teamCount = count;
    public IEnumerable<SlotData> GetTargetableSlot(Team team, Side side)
    {
        Team targetTeam = EnumCaster.GetTargetTeam(team, side);

        if (targetTeam == Team.All) return GetAllSlots();
        else return GetTeamSlots(targetTeam);
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
