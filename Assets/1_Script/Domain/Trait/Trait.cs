public enum Side { Self, Opponent, All }
public enum TargetRange
{
    None,
    Single,
    All,
}

public struct TraitTargetRule
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
    public readonly Side TargetSide;
    public readonly TargetRange TargetRange;
    public readonly ITraitAction TraitAction;

    public Trait(Side targetSide, TargetRange targetRange, ITraitAction action)
    {
        TargetSide = targetSide;
        TargetRange = targetRange;
        TraitAction = action;
    }
}

public class TraitActor
{
    readonly ITraitAction action;
    readonly ITraitCondition condition;

    public TraitActor(ITraitAction action, ITraitCondition condition)
    {
        this.action = action;
        this.condition = condition;
    }

    public void DoTrait(Champion target)
    {
        if (condition.Condition(target.StatData))
            target.OnTrait(action);
    }
}