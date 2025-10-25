using System.Collections.Generic;

public class TraitUsePersenter
{
    readonly SlotStorage<IEnumerable<SkillData>> traitSlots;
    TraitTargetSelector traitTargetSelector;
    public IEnumerable<SlotData> CurrentTargets => traitTargetSelector?.Targets;
    SkillUseController facade;
    int TeamSize;
    public TraitUsePersenter(SkillUseController facade, int teamSize, SlotStorage<IEnumerable<SkillData>> traitDatas)
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
            facade.UseSkill(useSlot.Value, traitTargetSelector.Targets, traitSlots.GetSlot(useSlot.Value));
            useSlot = null;
            return true;
        }
        else return false;
    }

    public bool IsUseable => useSlot.HasValue;
    public SlotData UseSlot => useSlot.Value;
    SlotData? useSlot;
    public void SelectUseTrait(SlotData useSlot, TraitTargetRule rule)
    {
        this.useSlot = useSlot;
        traitTargetSelector = new TraitTargetSelector(TeamSize, rule);
    }
}
