public enum Side { Self, Opponent, All }
public enum TraitTargetType
{
    None,
    Single,
    All,
}
public class Trait
{
    public readonly Side TargetSide;
    public readonly ITraitAction TraitAction;

    public Trait(Side targetSide, ITraitAction action)
    {
        TargetSide = targetSide;
        TraitAction = action;
    }
}