

public enum Side { Self, Opponent, All }
public enum TargetRange
{
    None,
    Single,
    All,
}

public readonly struct TraitTargetRule
{
    public readonly Side TargetSide;
    public readonly TargetRange TargetRange;

    public TraitTargetRule(Side targetSide, TargetRange targetRange)
    {
        TargetSide = targetSide;
        TargetRange = targetRange;
    }
}

public class TraitData
{
    public readonly TraitType TraitType;
    public readonly int Amount;

    public readonly TraitConditionType ConditionType;
    public readonly int Threshold;

    public TraitData(TraitType traitType, int amount, TraitConditionType conditionType = TraitConditionType.None, int threshold = 0)
    {
        TraitType = traitType;
        Amount = amount;
        ConditionType = conditionType;
        Threshold = threshold;
    }
}

public class TraitExecutor
{
    readonly TraitConditionChecker conditionChecker = new TraitConditionChecker();
    readonly ITraitAction action;
    public TraitExecutor(ITraitAction traitAction) => action = traitAction;

    public void ExecuteTrait(Champion champion, TraitData traitData)
    {
        if (CanExecute(champion, traitData))
            action.Do(champion);
    }

    bool CanExecute(Champion champion, TraitData traitData) =>
        conditionChecker.CheckCondition(traitData.ConditionType, champion.StatData, traitData.Threshold);
}