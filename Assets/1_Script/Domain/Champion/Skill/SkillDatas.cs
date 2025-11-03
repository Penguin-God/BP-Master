
public enum SkillType
{
    None,
    AttackChanger,
    PercentAttackChanger,
    DefenseChanger,
    PercentDefenseChanger,
    SpeedChanger,
    DefenseFixer,
    TraitExcluder,
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

public enum ConditionType
{
    None,
    Threshold,
    Compare,
    Trait,
}

public readonly struct SkillConditionData
{
    public readonly StatConditionType StatType;
    public readonly int Threshold;
    public readonly ConditionType ConditionType;
    public readonly TraitType TraitType;

    public SkillConditionData(StatConditionType statType, int threshold, TraitType traitType, ConditionType checkerType)
    {
        StatType = statType;
        Threshold = threshold;
        ConditionType = checkerType;
        TraitType = traitType;
    }
}

public readonly struct SkillData
{
    public readonly SkillType TraitType;
    public readonly int Amount;
    public readonly SkillConditionData ConditionData;
    public readonly TraitTargetRule TargetRule;

    public SkillData(SkillType traitType, int amount, SkillConditionData conditionData, TraitTargetRule traitTargetRule)
    {
        TraitType = traitType;
        Amount = amount;
        TargetRule = traitTargetRule;
        ConditionData = conditionData;
    }
}