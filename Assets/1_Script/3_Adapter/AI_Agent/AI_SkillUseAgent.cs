
public class AI_SkillUseAgent
{
    readonly SlotStorage<Skill> skillSlots;
    readonly SkillUsecase skillUseController;
    readonly RandomSkillTargetSelector targetSelector = new RandomSkillTargetSelector();
    public AI_SkillUseAgent(SlotStorage<Skill> skillSlots, SkillUsecase skillUseController)
    {
        this.skillSlots = skillSlots;
        this.skillUseController = skillUseController;
    }

    public void UseSkill(SlotData slotData)
    {
        var teamCount = skillSlots.GetTeamCounter();
        var useSkill = skillSlots.GetSlot(slotData);
        var targets = targetSelector.SelectSkillTargets(slotData.Team, useSkill, teamCount);
        skillUseController.UseSkill(slotData, targets, useSkill);
    }
}