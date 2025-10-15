using System.Collections.Generic;
using System.Linq;
using System;

public class AI_TraitAgent
{
    readonly TraitUseFacade traitUseFacade;
    readonly TraitSlotFilter traitSlotFilter;
    readonly SlotStorage<IEnumerable<TraitData>> traits;
    readonly Team Team;

    public AI_TraitAgent(Team team, TraitSlotFilter traitSlotFilter, SlotStorage<IEnumerable<TraitData>> traits, TraitUseFacade traitUseFacade)
    {
        Team = team;
        this.traitSlotFilter = traitSlotFilter;
        this.traits = traits;
        this.traitUseFacade = traitUseFacade;
    }

    public void UseTrait(Team team)
    {
        if (Team != team) return;
        var usableSlots = traitSlotFilter.FilteringUseableSlots(Team).ToList();

        var random = new Random();
        SlotData useSlot = usableSlots[random.Next(usableSlots.Count)];
        IEnumerable<TraitData> useDatas = traits.GetSlot(useSlot);

        var targetSides = useDatas.Select(x => x.TargetRule.TargetSide);
        var targetSlots = traitSlotFilter.FilteringTargetSlots(Team, targetSides).ToList();
        SlotData targetSlot = targetSlots[random.Next(targetSlots.Count)];

        traitUseFacade.UseTrait(useSlot, targetSlot, useDatas);
    }
}
