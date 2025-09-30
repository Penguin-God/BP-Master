using System.Collections.Generic;
using System.Linq;

public class TraitSlotFilter
{
    readonly int TeamSize;
    readonly TraitUseFacade traitUseFacade;
    public TraitSlotFilter(int teamSize, TraitUseFacade traitUseFacade)
    {
        TeamSize = teamSize;
        this.traitUseFacade = traitUseFacade;
    }

    public IEnumerable<SlotData> FilteringUseableSlots(Team team)
    {
        return Enumerable.Range(0, TeamSize)
                 .Select(i => new SlotData(team, i))
                 .Where(slot => traitUseFacade.IsTraitUsed(slot) == false);
    }

    public IEnumerable<SlotData> FilteringTargetSlots(Team team, Side side) => new TraitTargetSelector(TeamSize).GetTargetableSlot(team, side);
}
