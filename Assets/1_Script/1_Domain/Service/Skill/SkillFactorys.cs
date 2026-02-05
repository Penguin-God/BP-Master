using System;

public interface ISkillActionFactory
{
    ISkillAction CreateAction(SkillType actionType, SkillAmountData amountData, ChampionStatus caster);
}

public class SkillExecutorFactory
{
    readonly ISkillActionFactory skillActionFactory;
    public SkillExecutorFactory(ISkillActionFactory skillActionFactory)
    {
        this.skillActionFactory = skillActionFactory;
    }

    public SkillExecutor CreateExecutor(SkillData skillData, ChampionStatus useChamp)
    {
        ISkillAction action = skillActionFactory.CreateAction(skillData.SkillType, skillData.AmountData, useChamp);
        IChampionCondition condition = new SkillCondtionFactory().CreateCondition(skillData.ConditionData, useChamp.Stat);
        return new SkillExecutor(action, condition);
    }
}

public class SkillCondtionFactory
{
    public IChampionCondition CreateCondition(SkillConditionData data, ChampionStatData useChamp)
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