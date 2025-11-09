using System.Collections.Generic;

public class SkillUsePersenter
{
    readonly SlotStorage<IEnumerable<SkillData>> traitSlots;
    TraitTargetSelector traitTargetSelector;
    public IEnumerable<SlotData> CurrentTargets => traitTargetSelector?.Targets;
    SkillUseController useController;
    int TeamSize;
    public SkillUsePersenter(SkillUseController facade, int teamSize, SlotStorage<IEnumerable<SkillData>> traitDatas)
    {
        this.useController = facade;
        TeamSize = teamSize;
        traitSlots = traitDatas;
    }

    public void SelectTarget(SlotData targetSlot)
    {
        if (IsUseable == false) return;

        traitTargetSelector.Select(targetSlot);
        if (traitTargetSelector.IsFull)
        {
            useController.UseSkill(useSlot.Value, traitTargetSelector.Targets, traitSlots.GetSlot(useSlot.Value));
            useSlot = null;
        }
    }

    public bool IsUseable => useSlot.HasValue;
    public SlotData UseSlot => useSlot.Value;
    SlotData? useSlot;
    public void SelectUseSkill(SlotData useSlot, TraitTargetRule rule)
    {
        this.useSlot = useSlot;
        traitTargetSelector = new TraitTargetSelector(TeamSize, rule);
    }
}
