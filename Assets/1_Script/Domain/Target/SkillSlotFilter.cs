using System.Collections.Generic;
using System.Linq;

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

    public IEnumerable<SlotData> FilteringTargetSlots(Team team, IEnumerable<Side> sides, int teamSize)
    {
        var side = EnumCaster.MergeSide(sides);
        return new SkillTargetFinder(teamSize).GetTargetableSlot(team, side);
    }
}
