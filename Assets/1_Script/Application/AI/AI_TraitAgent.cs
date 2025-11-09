using System.Collections.Generic;
using System.Linq;

public class AI_TraitAgent
{
    readonly Team Team;
    readonly SkillUseController skillController;
    readonly SkillSlotFilter skillSlotFilter;
    readonly SlotStorage<Skill> skillSlots;
    readonly TargetCounter targetCounter;
    readonly AI_SKillDicision sKillDicision = new AI_SKillDicision();

    public AI_TraitAgent(Team team, SkillSlotFilter skillSlotFilter, SlotStorage<Skill> skills, SkillUseController skillController, TargetCounter targetCounter)
    {
        Team = team;
        this.skillSlotFilter = skillSlotFilter;
        this.skillSlots = skills;
        this.skillController = skillController;
        this.targetCounter = targetCounter;
    }

    public void UseTrait(Team team)
    {
        if (Team != team) return;
        Skill useSkill = sKillDicision.SelectSkill(skillSlots.GetTeam(Team));

        var targetSides = useSkill.Sides;
        var targetSlots = skillSlotFilter.FilteringTargetSlots(Team, targetSides).ToList();

        int targetCount = targetCounter.CalculateTargetCount(EnumCaster.MergeRule(useSkill.Rules));
        skillController.UseSkill(SkillToSlot(useSkill), SelectSkillTarget(targetSlots, targetCount), useSkill.SkillDatas);
    }

    SlotData SkillToSlot(Skill skill) => skillSlots.GetAllSlotDatas().First(x => skillSlots.GetSlot(x) == skill);

    IEnumerable<SlotData> SelectSkillTarget(List<SlotData> targetSlots, int targetCount)
    {
        List<SlotData> result = new();
        for (int i = 0; i < targetCount; i++)
        {
            var target = RandomUtil.DrawRandom(targetSlots);
            result.Add(target);
            targetSlots.Remove(target);
        }
        return result;
    }
}
