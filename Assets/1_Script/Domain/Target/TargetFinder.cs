using System.Collections.Generic;
using System.Linq;

public class SkillTargetFinder
{
    readonly int teamCount;
    public SkillTargetFinder(int count) => teamCount = count;
    public IEnumerable<SlotData> GetTargetableSlot(Team team, Side side)
    {
        Team targetTeam = EnumCaster.GetTargetTeam(team, side);

        if (targetTeam == Team.All) return GetAllSlots();
        else return GetTeamSlots(targetTeam);
    }

    IEnumerable<SlotData> GetTeamSlots(Team team) => Enumerable.Range(0, teamCount).Select(i => new SlotData(team, i));
    IEnumerable<SlotData> GetAllSlots() => new[] { Team.Blue, Team.Red }.SelectMany(x => GetTeamSlots(x));
}
