using System.Collections.Generic;

public class TraitUseFacade
{
    readonly SlotStorage<TraitApplier> appliers;
    public TraitUseFacade(SlotStorage<TraitApplier> appliers) => this.appliers = appliers;

    public void UseTrait(SlotData traitSlot, SlotData targetSlot, IEnumerable<TraitData> traitDatas)
    {
        foreach (var data in traitDatas)
            appliers.GetSlot(traitSlot).Execute(data, targetSlot);
    }
}