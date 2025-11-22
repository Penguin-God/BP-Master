using System.Collections.Generic;
using System.Linq;

public class AI_SkillAgent
{
    readonly Team Team;
    readonly SkillUseController skillController;
    readonly SkillSlotFilter skillSlotFilter;
    readonly SlotStorage<Skill> skillSlots;
    readonly TargetCounter targetCounter;
    readonly AI_SKillDicision sKillDicision = new AI_SKillDicision();

    public AI_SkillAgent(Team team, SkillSlotFilter skillSlotFilter, SlotStorage<Skill> skills, SkillUseController skillController, TargetCounter targetCounter)
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
        var usableSlots = skillSlotFilter.FilteringUseableSlots(Team).ToList();
        Skill useSkill = sKillDicision.SelectSkill(usableSlots.Select(x => skillSlots.GetSlot(x)));

        var targetSides = useSkill.Sides;
        var targetSlots = skillSlotFilter.FilteringTargetSlots(Team, targetSides).ToList();

        int targetCount = targetCounter.CalculateTargetCount(EnumCaster.MergeRule(useSkill.Rules));
        skillController.UseSkill(SkillToSlot(useSkill), SelectSkillTarget(targetSlots, targetCount), useSkill);
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
