
public enum TraitType
{
    None,
    AttackChanger,
    DefenseChanger,
    SpeedChanger,
}

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

    public readonly TraitTargetRule TargetRule;

    public TraitData(TraitType traitType, int amount, TraitConditionType conditionType, int threshold, TraitTargetRule traitTargetRule)
    {
        TraitType = traitType;
        Amount = amount;
        ConditionType = conditionType;
        Threshold = threshold;
        TargetRule = traitTargetRule;
    }
}

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