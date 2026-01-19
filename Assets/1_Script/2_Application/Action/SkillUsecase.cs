using System;
using System.Collections.Generic;
using System.Linq;

public class SkillUseController
{
    private readonly SlotStorage<ChampionStatus> _statusSlots;
    private readonly SkillRunner _skillRunner;
    public event Action<SlotData> OnUseSkill;

    public SkillUseController(SlotStorage<ChampionStatus> statusSlots, SkillRunner skillRunner)
    {
        _statusSlots = statusSlots;
        _skillRunner = skillRunner;
    }

    public void UseSkill(SlotData skillSlot, IEnumerable<SlotData> targetSlots, Skill skill)
    {
        var caster = _statusSlots.GetSlot(skillSlot);
        var targets = targetSlots.Select(x => _statusSlots.GetSlot(x));

        // 복잡한 순회 로직 대신 SkillRunner를 호출합니다.
        _skillRunner.Run(skill, caster, targets);

        OnUseSkill?.Invoke(skillSlot);
    }
}