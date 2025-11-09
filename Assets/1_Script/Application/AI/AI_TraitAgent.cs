using System.Collections.Generic;
using System.Linq;

public class AI_TraitAgent
{
    readonly Team Team;
    readonly SkillUseController skillController;
    readonly SkillSlotFilter skillSlotFilter;
    readonly SlotStorage<IEnumerable<SkillData>> skills;
    readonly TargetCounter targetCounter;
    readonly AI_SKillDicision sKillDicision = new AI_SKillDicision();

    public AI_TraitAgent(Team team, SkillSlotFilter skillSlotFilter, SlotStorage<IEnumerable<SkillData>> skills, SkillUseController skillController, TargetCounter targetCounter)
    {
        Team = team;
        this.skillSlotFilter = skillSlotFilter;
        this.skills = skills;
        this.skillController = skillController;
        this.targetCounter = targetCounter;
    }

    public void UseTrait(Team team)
    {
        if (Team != team) return;
        var usableSlots = skillSlotFilter.FilteringUseableSlots(Team).ToList();

        SlotData useSlot = RandomUtil.DrawRandom(usableSlots);
        IEnumerable<SkillData> useDatas = skills.GetSlot(useSlot);

        var targetSides = useDatas.Select(x => x.TargetRule.TargetSide);
        var targetSlots = skillSlotFilter.FilteringTargetSlots(Team, targetSides).ToList();

        int targetCount = targetCounter.CalculateTargetCount(EnumCaster.MergeRule(useDatas.Select(x => x.TargetRule)));
        skillController.UseSkill(useSlot, SelectSkillTarget(targetSlots, targetCount), useDatas);
    }

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
