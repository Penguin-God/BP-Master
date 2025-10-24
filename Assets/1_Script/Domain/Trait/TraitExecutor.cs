
using System.Collections.Generic;
using System.Linq;

public class TraitExecutor
{
    readonly ITraitAction action;
    readonly ITraitConditionChecker conditionChecker;
    public TraitExecutor(ITraitAction traitAction, ITraitConditionChecker checker)
    {
        action = traitAction;
        conditionChecker = checker;
    }

    public void ExecuteTrait(ChampionStatus target)
    {
        if (CanExecute(target))
            action.Do(target);
    }

    public void ExecuteTrait(IEnumerable<ChampionStatus> targets)
    {
        foreach (ChampionStatus target in targets.Where(x => CanExecute(x)))
            action.Do(target);
    }

    bool CanExecute(ChampionStatus target) => conditionChecker.Check(target.Stat) && target.IsTraitExcluded == false;
}