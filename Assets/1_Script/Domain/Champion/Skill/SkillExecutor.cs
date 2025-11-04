using System.Collections.Generic;
using System.Linq;

public class SkillExecutor
{
    readonly ISkillAction action;
    readonly IChampionCondition condition;
    public SkillExecutor(ISkillAction traitAction, IChampionCondition checker)
    {
        action = traitAction;
        condition = checker;
    }

    public void ExecuteSkill(IEnumerable<ChampionStatus> targets)
    {
        foreach (ChampionStatus target in targets.Where(x => CanExecute(x)))
            action.Do(target);
    }

bool CanExecute(ChampionStatus target) => condition.Check(target) && target.IsSkillExcluded == false;
}