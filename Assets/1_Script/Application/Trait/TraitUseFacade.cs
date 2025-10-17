using System;
using System.Collections.Generic;

public class TraitUseFacade
{
    readonly SlotStorage<TraitApplier> appliers;

    public event Action<SlotData> OnTraitUsed;

    public TraitUseFacade(SlotStorage<TraitApplier> appliers) => this.appliers = appliers;

    public void UseTrait(SlotData traitSlot, SlotData targetSlot, IEnumerable<TraitData> traitDatas)
    {
        foreach (var data in traitDatas)
            appliers.GetSlot(traitSlot).Execute(data, targetSlot, traitSlot);
        OnTraitUsed?.Invoke(traitSlot);
    }
}