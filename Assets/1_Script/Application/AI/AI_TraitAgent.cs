using System;
using System.Collections.Generic;
using System.Linq;

public class AI_TraitAgent
{
    readonly TraitUseFacade traitUseFacade;
    readonly TraitSlotFilter traitSlotFilter;
    readonly SlotStorage<IEnumerable<TraitData>> traits;
    readonly Team Team;

    readonly TargetCounter targetCounter;
    public AI_TraitAgent(Team team, TraitSlotFilter traitSlotFilter, SlotStorage<IEnumerable<TraitData>> traits, TraitUseFacade traitUseFacade, TargetCounter targetCounter)
    {
        Team = team;
        this.traitSlotFilter = traitSlotFilter;
        this.traits = traits;
        this.traitUseFacade = traitUseFacade;
        this.targetCounter = targetCounter;
    }

    Random random = new Random();
    public void UseTrait(Team team)
    {
        if (Team != team) return;
        var usableSlots = traitSlotFilter.FilteringUseableSlots(Team).ToList();

        
        SlotData useSlot = usableSlots[random.Next(usableSlots.Count)];
        IEnumerable<TraitData> useDatas = traits.GetSlot(useSlot);

        var targetSides = useDatas.Select(x => x.TargetRule.TargetSide);
        var targetSlots = traitSlotFilter.FilteringTargetSlots(Team, targetSides).ToList();

        int targetCount = targetCounter.CalculateTargetCount(EnumCaster.MergeRule(useDatas.Select(x => x.TargetRule)));
        traitUseFacade.UseTrait(useSlot, SelectSlots(targetSlots, targetCount), useDatas);
    }

    IEnumerable<SlotData> SelectSlots(List<SlotData> targetSlots, int targetCount)
    {
        List<SlotData> result = new();
        for (int i = 0; i < targetCount; i++)
        {
            var target = targetSlots[random.Next(targetSlots.Count)];
            result.Add(target);
            targetSlots.Remove(target);
        }
        return result;
    }
}
