using System;
using System.Collections.Generic;

public class TraitUseFacade
{
    readonly SlotStorage<ChampionStatus> statuses;
    readonly TraitApplier applier;
    readonly SlotStorage<TraitApplier> appliers;

    public event Action<SlotData> OnTraitUsed;

    public TraitUseFacade(SlotStorage<ChampionStatus> statuses)
    {
        this.statuses = statuses;
        applier = new TraitApplier(statuses);
    }

    public TraitUseFacade(SlotStorage<TraitApplier> appliers)
    {
        this.appliers = appliers;
    }

    public void UseTrait(SlotData traitSlot, SlotData targetSlot, IEnumerable<TraitData> traitDatas)
    {
        foreach (var data in traitDatas)
            appliers.GetSlot(traitSlot).Execute(data, targetSlot);
        OnTraitUsed?.Invoke(traitSlot);
    }

    public bool IsTraitUsed(SlotData slot) => appliers.GetSlot(slot).IsUse;
}