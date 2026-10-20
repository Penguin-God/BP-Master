using System;

public class AI_SkillExecutionUseCase
{
    readonly SkillUsecase _skillUseController;
    readonly Func<ChampionStatus, int> _targetOrder;

    public AI_SkillExecutionUseCase(SkillUsecase skillUseController, Func<ChampionStatus, int> targetOrder)
    {
        _skillUseController = skillUseController;
        _targetOrder = targetOrder;
    }

    public void UseSkill(SlotData slotData) => _skillUseController.AutoUseSkill(slotData, _targetOrder);
}