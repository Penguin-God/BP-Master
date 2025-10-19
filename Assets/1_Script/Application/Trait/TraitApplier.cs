using System.Collections.Generic;
using System.Linq;
using System;

public class TraitApplier
{
    readonly SlotStorage<ChampionStatus> statuses;
    readonly TraitTargetFinder targetSelector;
    public bool IsUse { get; set; }

    readonly SlotData Slot;
    public TraitApplier(SlotStorage<ChampionStatus> statuses, SlotData slotData)
    {
        this.statuses = statuses;
        Slot = slotData;

        int teamSize = statuses.GetTeam(Team.Blue).Count();
        targetSelector = new TraitTargetFinder(teamSize);
    }

    public void Execute(TraitData traitData, SlotData targetSlot)
    {
        var executor = new TraitExecutorFactory().CreateExecutor(traitData, statuses.GetSlot(Slot).Stat);
        IEnumerable<SlotData> targetSlots = targetSelector.GetTargetSlots(traitData.TargetRule, targetSlot);

        foreach (var slot in targetSlots)
        {
            var target = statuses.GetSlot(slot);
            executor.ExecuteTrait(target);
        }
        IsUse = true;
    }

    public void Execute(TraitData traitData, IEnumerable<SlotData> targetSlots)
    {
        var executor = new TraitExecutorFactory().CreateExecutor(traitData, statuses.GetSlot(Slot).Stat);
        foreach (var slot in targetSlots)
        {
            var target = statuses.GetSlot(slot);
            executor.ExecuteTrait(target);
        }
        IsUse = true;
    }
}
