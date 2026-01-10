using System;
using System.Collections.Generic;
using System.Linq;

public class SkillUseController
{
    readonly SlotStorage<ChampionStatus> statusSlots;
    readonly SkillExecutorFactory skillExecutorFactory;
    public event Action<SlotData> OnUseSkill;

    public SkillUseController(SlotStorage<ChampionStatus> statusSlots, SkillExecutorFactory skillExecutorFactory)
    {
        this.statusSlots = statusSlots;
        this.skillExecutorFactory = skillExecutorFactory;
    }

    public void UseSkill(SlotData skillSlot, IEnumerable<SlotData> targetSlots, Skill skill)
    {
        var targets = targetSlots.Select(x => statusSlots.GetSlot(x));
        foreach (var skillData in skill.SkillDatas)
        {
            var executor = skillExecutorFactory.CreateExecutor(skillData, statusSlots.GetSlot(skillSlot));
            executor.ExecuteSkill(targets);
        }
        OnUseSkill?.Invoke(skillSlot);
    }
}