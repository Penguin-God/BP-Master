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
public record SkillAmountData(AmountType Type, StatType StatType, int ValueAmount, float PercentValue, int FixValue);

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