using System;
using System.Collections.Generic;
using System.Linq;

public class AI_SkillTargetSelector
{
    public IEnumerable<SlotData> SelectSkillTargets(Team casterTeam, Skill skill, TargetCountCalculator calculator)
    {
        int count = calculator.CalculateTargetCount(casterTeam, EnumCaster.MergeRule(skill.Rules));
        var filter = new SkillTargetFilter(calculator);
        var candidates = filter.FilteringTargetSlots(casterTeam, skill.Sides);

        return SelectRandom(candidates, count);
    }

    // 이렇게 고르는 부분은 다형성으로
    public IEnumerable<SlotData> SelectRandom(IEnumerable<SlotData> candidates, int count) => candidates.OrderBy(x => Guid.NewGuid()).Take(count);
}

public interface ISkillTargetSelector
{
    
}