using System.Collections.Generic;
using System.Linq;

public class TraitSlotFilter
{
    readonly int TeamSize;
    SlotStorage<bool> triatUseFlagSlots;
    public TraitSlotFilter(SlotStorage<bool> triatUseFlagSlots)
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
        return new TraitTargetFinder(TeamSize).GetTargetableSlot(team, side);
    }

    public IEnumerable<SlotData> GetSlots(bool isUse, Team team, IEnumerable<Side> sides)
    {
        if (isUse) return FilteringTargetSlots(team, sides);
        else return FilteringUseableSlots(team);
    }
}
