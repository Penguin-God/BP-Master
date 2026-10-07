using System;
using System.Collections.Generic;
using System.Linq;

public class SkillUsecase
{
    readonly SlotStorage<Champion> pickChampions;
    readonly SkillRunner _skillRunner;
    public event Action<SlotData> OnUseSkill;

    public SkillUsecase(SlotStorage<Champion> pickChampions, SkillRunner skillRunner)
    {
        this.pickChampions = pickChampions;
        _skillRunner = skillRunner;
    }

    public void UseSkill(SlotData skillSlot, IEnumerable<SlotData> targetSlots)
    {
        var champion = pickChampions.GetSlot(skillSlot);
        var targets = targetSlots.Select(x => pickChampions.GetSlot(x).Status);

        _skillRunner.Run(champion.Skill, champion.Status, targets, skillSlot.Team);

        OnUseSkill?.Invoke(skillSlot);
    }
}