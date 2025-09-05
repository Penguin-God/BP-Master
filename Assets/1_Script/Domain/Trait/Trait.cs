public enum Side { Self, Opponent, All }
public enum TargetRange
{
    None,
    Single,
    All,
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