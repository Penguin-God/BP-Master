
public enum SkillType
{
    None,
    AttackChanger,
    DefenseChanger,
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

public class SkillData
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