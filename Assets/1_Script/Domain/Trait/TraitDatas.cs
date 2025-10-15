
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

public readonly struct TraitConditionData
{
    public readonly TraitConditionType ConditionType;
    public readonly int Threshold;

    public TraitConditionData(TraitConditionType type, int threshold)
    {
        ConditionType = type;
        Threshold = threshold;
    }
}

public class TraitData
{
    public readonly TraitType TraitType;
    public readonly int Amount;

    public TraitConditionType ConditionType => ConditionData.ConditionType;
    public int Threshold => ConditionData.Threshold;
    public readonly TraitConditionData ConditionData;

    public readonly TraitTargetRule TargetRule;

    public TraitData(TraitType traitType, int amount, TraitConditionType conditionType, int threshold, TraitTargetRule traitTargetRule)
    {
        TraitType = traitType;
        Amount = amount;
        TargetRule = traitTargetRule;
        ConditionData = new TraitConditionData(conditionType, threshold);
    }
}