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
