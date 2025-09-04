public enum Side { Self, Opponent, All }

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