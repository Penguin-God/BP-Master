public readonly struct CardSelectRule
{
    public readonly Side TargetSide;
    public readonly TargetRange TargetRange;

    public CardSelectRule(Side targetSide, TargetRange targetRange)
    {
        TargetSide = targetSide;
        TargetRange = targetRange;
    }
}

public enum SkillApplyType { Select, OneSelf, }
public enum SkillValueType { Value, SelfRef, SelectRef }

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
    public readonly CardSelectRule SelectRule;
    public readonly SkillApplyType SkillApplyType;
    public readonly SkillValueType SkillValueType;

    public SkillData(SkillType skillType, SkillAmountData amountData, SkillConditionData conditionData, CardSelectRule selectRule)
    {
        SkillType = skillType;
        AmountData = amountData;
        SelectRule = selectRule;
        ConditionData = conditionData;
        SkillApplyType = 0;
        SkillValueType = 0;
    }

    public SkillData(SkillType skillType, SkillAmountData amountData, SkillConditionData conditionData, CardSelectRule selectRule, SkillApplyType skillApplyType, SkillValueType skillValueType)
    {
        SkillType = skillType;
        AmountData = amountData;
        SelectRule = selectRule;
        ConditionData = conditionData;
        SkillApplyType = skillApplyType;
        SkillValueType = skillValueType;
    }
}