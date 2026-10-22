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
public record SkillViewModel(SkillType SkillType, SkillAmountData AmountData, SkillConditionData ConditionData, CardSelectRule SelectRule);

public readonly struct SkillData
{
    public readonly SkillType SkillType;
    public readonly ISkillAmountCalculator Calculator;
    public readonly StatType StatType;
    public readonly IChampionCondition Condition;
    public readonly CardSelectRule SelectRule;
    public readonly SkillApplyType SkillApplyType;
    public readonly SkillValueType SkillValueType;

    public SkillData(SkillType skillType, ISkillAmountCalculator calculator, IChampionCondition condition, CardSelectRule selectRule, SkillApplyType skillApplyType, SkillValueType skillValueType, StatType statType)
    {
        SkillType = skillType;
        Calculator = calculator;
        SelectRule = selectRule;
        Condition = condition;
        SkillApplyType = skillApplyType;
        SkillValueType = skillValueType;
        StatType = statType;
    }
}
