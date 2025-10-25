using System;
using System.Collections.Generic;
using System.Linq;

public class SkillUseController
{
    readonly SlotStorage<ChampionStatus> statusSlots;
    public event Action<SlotData> OnUseSkill;

    public SkillUseController(SlotStorage<ChampionStatus> statusSlots) => this.statusSlots = statusSlots;

    public void UseSkill(SlotData traitSlot, IEnumerable<SlotData> targetSlots, IEnumerable<SkillData> traitDatas)
    {
        var targets = targetSlots.Select(x => statusSlots.GetSlot(x));
        foreach (var trait in traitDatas)
        {
            var executor = new SkillExecutorFactory().CreateExecutor(trait, statusSlots.GetSlot(traitSlot).Stat);
            executor.ExecuteSkill(targets);
        }
        OnUseSkill?.Invoke(traitSlot);
    }
}