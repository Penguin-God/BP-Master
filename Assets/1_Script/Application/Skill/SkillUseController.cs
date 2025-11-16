using System;
using System.Collections.Generic;
using System.Linq;

public class SkillUseController
{
    readonly SlotStorage<ChampionStatus> statusSlots;
    public event Action<SlotData> OnUseSkill;

    public SkillUseController(SlotStorage<ChampionStatus> statusSlots) => this.statusSlots = statusSlots;

    public void UseSkill(SlotData skillSlot, IEnumerable<SlotData> targetSlots, Skill skill)
    {
        var targets = targetSlots.Select(x => statusSlots.GetSlot(x));
        foreach (var skillData in skill.SkillDatas)
        {
            var executor = new SkillExecutorFactory().CreateExecutor(skillData, statusSlots.GetSlot(skillSlot));
            executor.ExecuteSkill(targets);
        }
        OnUseSkill?.Invoke(skillSlot);
    }
}