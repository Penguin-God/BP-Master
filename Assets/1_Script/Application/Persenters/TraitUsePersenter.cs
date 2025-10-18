using System.Collections.Generic;

public class TraitUsePersenter
{
    readonly SlotStorage<TraitApplier> appliers;
    readonly SlotStorage<IEnumerable<TraitData>> traitSlots;
    TraitTargetSelector traitTargetSelector;
    int TeamSize;
    public TraitUsePersenter(SlotStorage<TraitApplier> appliers, int teamSize, SlotStorage<IEnumerable<TraitData>> traitDatas)
    {
        this.appliers = appliers;
        TeamSize = teamSize;
        traitSlots = traitDatas;
    }

    public bool SelectTarget(SlotData targetSlot)
    {
        if (IsUseable == false) return false;

        traitTargetSelector.Select(targetSlot);
        if (traitTargetSelector.IsFull)
        {
            foreach (var data in traitSlots.GetSlot(useSlot.Value))
                appliers.GetSlot(useSlot.Value).Execute(data, traitTargetSelector.Targets);
            return true;
        }
        else return false;
    }

    public bool IsUseable => useSlot.HasValue;
    SlotData? useSlot;
    public void SelectUseTrait(SlotData useSlot, TraitTargetRule rule)
    {
        this.useSlot = useSlot;
        traitTargetSelector = new TraitTargetSelector(TeamSize, rule);
    }
}
