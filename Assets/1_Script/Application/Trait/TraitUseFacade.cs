using System.Collections.Generic;

public class TraitUseFacade
{
    readonly SlotStorage<TraitApplier> appliers;
    readonly SlotStorage<ChampionStatus> statusSlots;
    public TraitUseFacade(SlotStorage<TraitApplier> appliers, SlotStorage<ChampionStatus> statusSlots)
    {
        this.appliers = appliers;
        this.statusSlots = statusSlots;
    }

    public void UseTrait(SlotData traitSlot, SlotData targetSlot, IEnumerable<TraitData> traitDatas)
    {
        foreach (var data in traitDatas)
            appliers.GetSlot(traitSlot).Execute(data, targetSlot);
    }

    public void UseTrait(SlotData traitSlot, IEnumerable<SlotData> targetSlots, IEnumerable<TraitData> traitDatas)
    {
        foreach (var data in traitDatas)
            appliers.GetSlot(traitSlot).Execute(data, targetSlots);
    }
}