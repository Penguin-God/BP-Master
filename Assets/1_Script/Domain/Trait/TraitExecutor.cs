
public class TraitExecutor
{
    readonly TraitConditionChecker conditionChecker = new TraitConditionChecker();
    readonly ITraitAction action;
    readonly TraitConditionData ConditionData;
    public TraitExecutor(ITraitAction traitAction, TraitConditionType conditionType, int threshold)
    {
        action = traitAction;
        ConditionData = new TraitConditionData(conditionType, threshold);
    }

    public void ExecuteTrait(ChampionStatus target)
    {
        if (CanExecute(target))
            action.Do(target);
    }

    bool CanExecute(ChampionStatus target) => conditionChecker.CheckCondition(ConditionData, target.Stat) && target.IsTraitExcluded == false;
}