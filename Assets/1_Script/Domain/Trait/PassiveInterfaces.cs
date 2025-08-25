
public interface ITraitAction
{
    public ChampionStatData Do(ChampionStatData stat);
}

public enum TraitType
{
    Passive,
    Active,
}
public class Trait
{
    public readonly TraitType TraitType;
    public readonly Side TargetSide;
    public readonly ITraitAction TraitAction;

    public Trait(TraitType traitType, Side targetSide, ITraitAction action)
    {
        TraitType = traitType;
        TargetSide = targetSide;
        TraitAction = action;
    }
}