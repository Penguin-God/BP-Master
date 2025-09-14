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

public class Trait
{
    public readonly TraitTargetRule TargetRule;
    public readonly ITraitAction TraitAction;

    public Trait(Side targetSide, TargetRange targetRange, ITraitAction action)
    {
        TargetRule = new TraitTargetRule(targetSide, targetRange);
        TraitAction = action;
    }
}

public class TraitExecutor
{
    public readonly ITraitAction Action;
    readonly TraitConditionType ConditionType;
    readonly int Threshold;
    readonly TraitConditionChecker conditionChecker = new TraitConditionChecker();
    public TraitExecutor(ITraitAction traitAction, TraitConditionType traitConditionType, int threshold)
    {
        Action = traitAction;
        ConditionType = traitConditionType;
        Threshold = threshold;
    }

    public void ExecteTrait(Champion champion)
    {
        if (conditionChecker.CheckCondition(ConditionType, champion.StatData, Threshold))
            Action.Do(champion);
    }
}