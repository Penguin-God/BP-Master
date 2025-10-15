
public class TraitExecutor
{
    readonly TraitConditionChecker conditionChecker = new TraitConditionChecker();
    readonly ITraitAction action;
    readonly TraitConditionType ConditionType;
    readonly int Threshold;

    public TraitExecutor(ITraitAction traitAction, TraitConditionType conditionType, int threshold)
    {
        action = traitAction;
        ConditionType = conditionType;
        Threshold = threshold;
    }

    public void ExecuteTrait(ChampionStatus target)
    {
        if (CanExecute(target))
            action.Do(target);
    }

    bool CanExecute(ChampionStatus target) => conditionChecker.CheckCondition(ConditionType, target.Stat, Threshold);
}