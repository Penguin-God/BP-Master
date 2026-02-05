using System;
using System.Collections.Generic;
using System.Linq;

public class SkillUsecase
{
    readonly SlotStorage<PickChampion> pickChampions;
    private readonly SkillRunner _skillRunner;
    public event Action<SlotData> OnUseSkill;

    public SkillUsecase(SlotStorage<PickChampion> pickChampions, SkillRunner skillRunner)
    {
        this.pickChampions = pickChampions;
        _skillRunner = skillRunner;
    }

    public void UseSkill(SlotData skillSlot, IEnumerable<SlotData> targetSlots)
    {
        var champion = pickChampions.GetSlot(skillSlot);
        var targets = targetSlots.Select(x => pickChampions.GetSlot(x).Status);

        _skillRunner.Run(champion.Skill, champion.Status, targets, champion.Team);

        OnUseSkill?.Invoke(skillSlot);
    }
}