using System;
using System.Collections.Generic;
using System.Linq;

public class AI_SkillTargetSelector
{
    public IEnumerable<SlotData> SelectSkillTargets(Team casterTeam, SkillData skillData, TargetCountCalculator calculator)
    {
        int count = calculator.CalculateTargetCount(casterTeam, skillData.TargetRule);
        var filter = new SkillTargetFilter(calculator);
        var candidates = filter.FilteringTargetSlots(casterTeam, new[] { skillData.TargetRule.TargetSide });

        return SelectRandom(candidates, count);
    }

    // 이렇게 고르는 부분은 다형성으로
    public IEnumerable<SlotData> SelectRandom(IEnumerable<SlotData> candidates, int count) => candidates.OrderBy(x => Guid.NewGuid()).Take(count);
}

public interface ISkillTargetSelector
{
    
}