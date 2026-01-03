using System.Linq;

public class AI_SkillUseAgent
{
    readonly SlotStorage<Skill> skillSlots;
    readonly SkillUseController skillUseController;
    readonly AI_SkillTargetSelector targetSelector = new AI_SkillTargetSelector();
    public AI_SkillUseAgent(SlotStorage<Skill> skillSlots, SkillUseController skillUseController)
    {
        this.skillSlots = skillSlots;
        this.skillUseController = skillUseController;
    }

    public void UseSkill(SlotData slotData)
    {
        var teamCount = skillSlots.GetTeamCounter();
        var filter = new SkillTargetFilter(teamCount);
        var useSkill = skillSlots.GetSlot(slotData);
        var targets = targetSelector.SelectRandom(filter.FilteringTargetSlots(slotData.Team, useSkill.Sides).ToList(), teamCount.CalculateTargetCount(slotData.Team, EnumCaster.MergeRule(useSkill.Rules)));
        skillUseController.UseSkill(slotData, targets, useSkill);
    }
}