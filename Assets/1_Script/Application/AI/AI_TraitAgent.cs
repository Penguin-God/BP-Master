using System;
using System.Collections.Generic;
using System.Linq;

public class AI_TraitAgent
{
    readonly Team Team;
    readonly SkillUseController skillController;
    readonly SkillSlotFilter skillSlotFilter;
    readonly SlotStorage<IEnumerable<SkillData>> traits;
    readonly TargetCounter targetCounter;

    public AI_TraitAgent(Team team, SkillSlotFilter skillSlotFilter, SlotStorage<IEnumerable<SkillData>> traits, SkillUseController skillController, TargetCounter targetCounter)
    {
        Team = team;
        this.skillSlotFilter = skillSlotFilter;
        this.traits = traits;
        this.skillController = skillController;
        this.targetCounter = targetCounter;
    }

    Random random = new Random();
    public void UseTrait(Team team)
    {
        if (Team != team) return;
        var usableSlots = skillSlotFilter.FilteringUseableSlots(Team).ToList();

        
        SlotData useSlot = usableSlots[random.Next(usableSlots.Count)];
        IEnumerable<SkillData> useDatas = traits.GetSlot(useSlot);

        var targetSides = useDatas.Select(x => x.TargetRule.TargetSide);
        var targetSlots = skillSlotFilter.FilteringTargetSlots(Team, targetSides).ToList();

        int targetCount = targetCounter.CalculateTargetCount(EnumCaster.MergeRule(useDatas.Select(x => x.TargetRule)));
        skillController.UseSkill(useSlot, SelectSlots(targetSlots, targetCount), useDatas);
    }

    IEnumerable<SlotData> SelectSlots(List<SlotData> targetSlots, int targetCount)
    {
        List<SlotData> result = new();
        for (int i = 0; i < targetCount; i++)
        {
            var target = targetSlots[random.Next(targetSlots.Count)];
            result.Add(target);
            targetSlots.Remove(target);
        }
        return result;
    }
}
