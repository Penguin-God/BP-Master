public class AI_SkillExecutionUseCase
{
    readonly SlotStorage<Skill> _skillSlots;
    readonly SkillUsecase _skillUseController;
    readonly ISkillTargetSelector _targetSelector;
    readonly SkillTargetService skillTargetService;
    public AI_SkillExecutionUseCase(SlotStorage<Skill> skillSlots, SkillUsecase skillUseController, ISkillTargetSelector targetSelector)
    {
        _skillSlots = skillSlots;
        _skillUseController = skillUseController;
        _targetSelector = targetSelector;
    }

    public void UseSkill(SlotData slotData)
    {
        var teamCount = _skillSlots.GetTeamCounter();
        var useSkill = _skillSlots.GetSlot(slotData);

        var targets = skillTargetService.GetTargets(slotData.Team, useSkill, teamCount);

        _skillUseController.UseSkill(slotData, targets);
    }
}