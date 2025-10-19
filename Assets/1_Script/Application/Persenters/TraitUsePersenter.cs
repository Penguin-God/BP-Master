using System.Collections.Generic;

public class TraitUsePersenter
{
    readonly SlotStorage<IEnumerable<TraitData>> traitSlots;
    TraitTargetSelector traitTargetSelector;
    TraitUseFacade facade;
    int TeamSize;
    public TraitUsePersenter(TraitUseFacade facade, int teamSize, SlotStorage<IEnumerable<TraitData>> traitDatas)
    {
        this.facade = facade;
        TeamSize = teamSize;
        traitSlots = traitDatas;
    }

    public bool SelectTarget(SlotData targetSlot)
    {
        if (IsUseable == false) return false;

        traitTargetSelector.Select(targetSlot);
        if (traitTargetSelector.IsFull)
        {
            facade.UseTrait(useSlot.Value, traitTargetSelector.Targets, traitSlots.GetSlot(useSlot.Value));
            useSlot = null;
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
