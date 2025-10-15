using NUnit.Framework.Internal.Filters;
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

    SlotStorage<TraitApplier> appliers;
    public TraitSlotFilter(SlotStorage<TraitApplier> appliers)
    {
        TeamSize = appliers.GetTeam(Team.Blue).Count();
        this.appliers = appliers;
    }

    public IEnumerable<SlotData> FilteringUseableSlots(Team team)
        => Enumerable.Range(0, TeamSize)
                 .Select(i => new SlotData(team, i))
                 .Where(slot => appliers.GetSlot(slot).IsUse == false);

    public IEnumerable<SlotData> FilteringTargetSlots(Team team, IEnumerable<Side> sides)
    {
        var side = BanPickEnumCaster.MergeSide(sides);
        return new TraitTargetSelector(TeamSize).GetTargetableSlot(team, side);
    }

    public IEnumerable<SlotData> GetSlots(bool isUse, Team team, IEnumerable<Side> sides)
    {
        if (isUse) return FilteringTargetSlots(team, sides);
        else return FilteringUseableSlots(team);
    }
}
