using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;

public static class SkillExecutor
{
    public static void ExecuteSkill(ISkillAction action, IChampionCondition condition, IEnumerable<ChampionStatus> targets)
        => targets
            .Where(condition.Check)
            .ForEach(target => action.Do(target));
}