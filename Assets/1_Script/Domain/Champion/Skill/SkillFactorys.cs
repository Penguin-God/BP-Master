using System;

public class SkillExecutorFactory
{
    public SkillExecutor CreateExecutor(SkillData skillData, ChampionStatus useChamp)
    {
        ISkillAction action = SkillActionFactory.CreateAction(skillData.TraitType, skillData.AmountData, useChamp);
        IChampionCondition condition = SkillCondtionFactory.CreateCondition(skillData.ConditionData, useChamp.Stat);
        return new SkillExecutor(action, condition);
    }
}

public static class SkillActionFactory
{
    public static ISkillAction CreateAction(SkillType actionType, SkillAmountData amountData, ChampionStatus useChamp)
    {
        var amountCalculator = SkillAmountCalculatorFactory.Create(amountData);
        return actionType switch
        {
            SkillType.AttackChanger => new AttackChanger(amountCalculator),
            SkillType.DefenseChanger => new DefenseChanger(amountCalculator),
            SkillType.SpeedChanger => new SpeedChanger(amountCalculator),
            SkillType.TraitExcluder => new SkillExcluder(),
            SkillType.DefenseAbsorber => new DefenseAbsorber(useChamp, amountCalculator),
            SkillType.Resonance => new Resonance(useChamp, amountData.PercentValue),
            SkillType.AmplifyChanger => new AmplifyChanger(amountData.PercentValue),
            _ => throw new NotImplementedException($"Action not implemented: {actionType}")
        };
    }
}

public static class SkillCondtionFactory
{
    public static IChampionCondition CreateCondition(SkillConditionData data, ChampionStatData useChamp)
    {
        return data.ConditionType switch
        {
            ConditionType.None => new NullChecker(),
            ConditionType.Threshold => new StatThresholdChecker(data.StatType, data.Threshold),
            ConditionType.Compare => new StatComparisonChecker(data.StatType, useChamp),
            ConditionType.Trait => new TraitCondition(data.TraitType),
            _ => throw new NotImplementedException($"Action not implemented: {data.ConditionType}")
        };
    }
}


public enum AmountType { None, Value, Percent, Fix }
public record SkillAmountData(AmountType Type, int ValueAmount, float PercentValue, int FixValue);


public static class SkillAmountCalculatorFactory
{
    public static ISkillAmountCalculator Create(SkillAmountData amountData)
        => amountData.Type switch
        {
            AmountType.Value => new ValueCalculator(amountData.ValueAmount),
            AmountType.Percent => new PercentCalculator(amountData.PercentValue),
            AmountType.Fix => new FixCalculator(amountData.FixValue),
            _ => null,
        };
}