using System;
using System.Collections.Generic;
using System.Linq;

public interface ISkillTargetSelector
{
    IEnumerable<SlotData> SelectSkillTargets(Team casterTeam, Skill skill, TargetCountCalculator calculator);
}

public class RandomSkillTargetSelector : ISkillTargetSelector
{
    public IEnumerable<SlotData> SelectSkillTargets(Team casterTeam, Skill skill, TargetCountCalculator calculator)
    {
        int count = calculator.CalculateTargetCount(casterTeam, EnumCaster.MergeRule(skill.Rules));
        var filter = new SkillTargetFilter(calculator);
        var candidates = filter.FilteringTargetSlots(casterTeam, skill.Sides);

        return SelectRandom(candidates, count);
    }

    public IEnumerable<SlotData> SelectRandom(IEnumerable<SlotData> candidates, int count) => candidates.OrderBy(x => Guid.NewGuid()).Take(count);
}


public class HighStatTargetSelector : ISkillTargetSelector
{
    readonly SlotStorage<ChampionStatus> statusStorage;

    public HighStatTargetSelector(SlotStorage<ChampionStatus> statusStorage)
    {
        this.statusStorage = statusStorage;
    }

    public IEnumerable<SlotData> SelectSkillTargets(Team casterTeam, Skill skill, TargetCountCalculator calculator)
    {
        int count = calculator.CalculateTargetCount(casterTeam, EnumCaster.MergeRule(skill.Rules));
        var filter = new SkillTargetFilter(calculator);
        var candidates = filter.FilteringTargetSlots(casterTeam, skill.Sides);

        var percentData = skill.SkillDatas.FirstOrDefault(x => x.AmountData.Type == AmountType.Percent);

        var statType = percentData.AmountData.StatType;
        return candidates
            .OrderByDescending(slot => statusStorage.GetSlot(slot).Stat.GetStatValue(statType))
            .ThenBy(x => Guid.NewGuid())
            .Take(count);
    }
}