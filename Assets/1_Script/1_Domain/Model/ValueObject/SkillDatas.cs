
public enum SkillType
{
    None = 0,
    AttackChanger = 1,
    StatChanger = 2,
    DefenseChanger = 3,
    SpeedChanger = 5,
    TraitExcluder = 7,
    DefenseAbsorber = 8,
    Resonance = 9,
    AmplifyChanger = 10,
    PickBuffer = 11,
    Doppelganger = 12,
    FinalStatChanger = 13,
}

public enum Side { Self, Opponent, All }
public enum TargetRange
{
    None,
    Single,
    Double,
    Triple,
    All,
}

public readonly struct SkillTargetRule
{
    public readonly Side TargetSide;
    public readonly TargetRange TargetRange;

    public SkillTargetRule(Side targetSide, TargetRange targetRange)
    {
        TargetSide = targetSide;
        TargetRange = targetRange;
    }
}

public enum ConditionType
{
    None,
    Threshold,
    Compare,
}

public readonly struct SkillConditionData
{
    public readonly StatConditionType StatType;
    public readonly int Threshold;
    public readonly ConditionType ConditionType;

    public SkillConditionData(StatConditionType statType, int threshold, ConditionType checkerType)
    {
        StatType = statType;
        Threshold = threshold;
        ConditionType = checkerType;
    }
}

public enum AmountType { None, Value, Percent, Fix }
public record SkillAmountData(AmountType Type, int ValueAmount, float PercentValue, int FixValue);

public readonly struct SkillData
{
    public readonly SkillType SkillType;
    public readonly SkillAmountData AmountData;
    public readonly SkillConditionData ConditionData;
    public readonly SkillTargetRule TargetRule;

    public SkillData(SkillType skillType, SkillAmountData amountData, SkillConditionData conditionData, SkillTargetRule traitTargetRule)
    {
        SkillType = skillType;
        AmountData = amountData;
        TargetRule = traitTargetRule;
        ConditionData = conditionData;
    }
}