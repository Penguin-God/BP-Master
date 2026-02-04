using System;

public interface ISkillActionFactory
{

}

public class SkillExecutorFactory
{
    readonly SkillActionFactory skillActionFactory;
    public SkillExecutorFactory(SkillActionFactory skillActionFactory)
    {
        this.skillActionFactory = skillActionFactory;
    }

    public SkillExecutor CreateExecutor(SkillData skillData, ChampionStatus useChamp, Team team)
    {
        ISkillAction action = skillActionFactory.CreateAction(skillData.SkillType, skillData.AmountData, useChamp, team);
        IChampionCondition condition = SkillCondtionFactory.CreateCondition(skillData.ConditionData, useChamp.Stat);
        return new SkillExecutor(action, condition);
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
            _ => throw new NotImplementedException($"Action not implemented: {data.ConditionType}")
        };
    }
}

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