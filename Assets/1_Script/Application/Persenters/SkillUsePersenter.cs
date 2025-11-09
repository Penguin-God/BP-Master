using System.Collections.Generic;

public class SkillUsePersenter
{
    TraitTargetSelector traitTargetSelector;
    public IEnumerable<SlotData> CurrentTargets => traitTargetSelector?.Targets;
    int TeamSize;
    public SkillUsePersenter(int teamSize)
    {
        TeamSize = teamSize;
    }

    public bool SelectTarget(SlotData targetSlot, out SlotData slotData)
    {
        slotData = default;
        if (IsUseable == false) return false;

        traitTargetSelector.Select(targetSlot);
        if (traitTargetSelector.IsFull)
        {
            slotData = UseSlot;
            useSlot = null;
            return true;
        }
        else return false;
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
