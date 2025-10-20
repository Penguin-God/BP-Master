using System;
using System.Collections.Generic;

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
        foreach (var data in traitDatas)
            appliers.GetSlot(traitSlot).Execute(data, targetSlots);

        OnUseTrait?.Invoke(traitSlot);
    }
}