using System;
using System.Collections.Generic;
using System.Linq;

public class SkillUsecase
{
    private readonly SlotStorage<ChampionStatus> _statusSlots;
    private readonly SkillRunner _skillRunner;
    public event Action<SlotData> OnUseSkill;

    public SkillUsecase(SlotStorage<ChampionStatus> statusSlots, SkillRunner skillRunner)
    {
        _statusSlots = statusSlots;
        _skillRunner = skillRunner;
    }

    public void UseSkill(SlotData skillSlot, IEnumerable<SlotData> targetSlots, Skill skill)
    {
        var caster = _statusSlots.GetSlot(skillSlot);
        var targets = targetSlots.Select(x => _statusSlots.GetSlot(x));

        _skillRunner.Run(skill, caster, targets);

        OnUseSkill?.Invoke(skillSlot);
    }
}