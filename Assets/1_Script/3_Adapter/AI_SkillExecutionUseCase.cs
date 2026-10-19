public class AI_SkillExecutionUseCase
{
    readonly SlotStorage<Skill> _skillSlots;
    readonly SkillUsecase _skillUseController;
    readonly ISkillTargetSelector _targetSelector;

    public AI_SkillExecutionUseCase(SlotStorage<Skill> skillSlots, SkillUsecase skillUseController, ISkillTargetSelector targetSelector)
    {
        _skillSlots = skillSlots;
        _skillUseController = skillUseController;
        _targetSelector = targetSelector;
    }

    public void UseSkill(SlotData slotData)
    {
        var useSkill = _skillSlots.GetSlot(slotData);
        var targets = Factorys.CreateTeamCounter(_skillSlots).GetTargets(slotData.Team, useSkill, _targetSelector);

        _skillUseController.UseSkill(slotData, targets);
    }
}