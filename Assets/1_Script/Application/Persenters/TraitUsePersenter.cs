using System.Collections.Generic;

public class TraitUsePersenter
{
    readonly SlotStorage<TraitApplier> appliers;
    TraitTargetSelector traitTargetSelector;
    int TeamSize;
    public TraitUsePersenter(SlotStorage<TraitApplier> appliers, int teamSize)
    {
        this.appliers = appliers;
        TeamSize = teamSize;
    }

    public bool UseTrait(SlotData targetSlot, IEnumerable<TraitData> traitDatas)
    {
        if (IsUseable == false) return false;

        traitTargetSelector.Select(targetSlot);
        if (traitTargetSelector.IsFull)
        {
            foreach (var data in traitDatas)
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
