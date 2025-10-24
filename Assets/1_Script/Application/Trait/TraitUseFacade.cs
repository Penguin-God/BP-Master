using System;
using System.Collections.Generic;
using System.Linq;

public class TraitUseFacade
{
    readonly SlotStorage<TraitApplier> appliers;
    readonly SlotStorage<ChampionStatus> statusSlots;
    public event Action<SlotData> OnUseTrait;

    public TraitUseFacade(SlotStorage<TraitApplier> appliers, SlotStorage<ChampionStatus> statusSlots)
    {
        this.appliers = appliers;
        this.statusSlots = statusSlots;
    }

    public void UseTrait(SlotData traitSlot, IEnumerable<SlotData> targetSlots, IEnumerable<TraitData> traitDatas)
    {
        var targets = targetSlots.Select(x => statusSlots.GetSlot(x));
        foreach (var trait in traitDatas)
        {
            var executor = new TraitExecutorFactory().CreateExecutor(trait, statusSlots.GetSlot(traitSlot).Stat);
            executor.ExecuteTrait(targets);
        }

        appliers.GetSlot(traitSlot).Use();
        OnUseTrait?.Invoke(traitSlot);
    }
}