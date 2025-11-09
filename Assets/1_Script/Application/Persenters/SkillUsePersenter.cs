using System.Collections.Generic;

public class SkillUsePersenter
{
    readonly SlotStorage<Skill> skillSlots;
    TraitTargetSelector traitTargetSelector;
    public IEnumerable<SlotData> CurrentTargets => traitTargetSelector?.Targets;
    SkillUseController useController;
    int TeamSize;
    public SkillUsePersenter(SkillUseController facade, int teamSize, SlotStorage<Skill> skillSlots)
    {
        this.useController = facade;
        TeamSize = teamSize;
        this.skillSlots = skillSlots;
    }

    public void SelectTarget(SlotData targetSlot)
    {
        if (IsUseable == false) return;

        traitTargetSelector.Select(targetSlot);
        if (traitTargetSelector.IsFull)
        {
            useController.UseSkill(useSlot.Value, traitTargetSelector.Targets, skillSlots.GetSlot(useSlot.Value).SkillDatas);
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
