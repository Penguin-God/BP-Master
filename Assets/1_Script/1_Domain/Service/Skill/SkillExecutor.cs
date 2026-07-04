using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;

public static class SkillExecutor
{
    public static void ExecuteSkill(ISkillAction action, IChampionCondition condition, IEnumerable<ChampionStatus> targets)
        => targets
            .Where(x => CanExecute(x, condition))
            .ForEach(target => action.Do(target));

    static bool CanExecute(ChampionStatus target, IChampionCondition condition) => condition.Check(target) && target.IsSkillExcluded == false;
}