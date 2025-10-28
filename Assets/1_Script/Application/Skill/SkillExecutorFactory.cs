using System;

public class SkillExecutorFactory
{
    public SkillExecutor CreateExecutor(SkillData traitData, ChampionStatData useChamp)
    {
        ISkillAction action = SkillActionFactory.CreateAction(traitData.TraitType, traitData.Amount);
        IChampionCondition condition = ChampionCondtionFactory.CreateChecker(traitData.ConditionData, useChamp);
        return new SkillExecutor(action, condition);
    }
}

public static class SkillActionFactory
{
    public static ISkillAction CreateAction(SkillType actionType, int amount)
    {
        return actionType switch
        {
            SkillType.AttackChanger => new AttackChanger(amount),
            SkillType.DefenseChanger => new DefenseChanger(amount),
            SkillType.SpeedChanger => new SpeedChanger(amount),
            SkillType.DefenseFixer => new DefenseFixer(amount),
            SkillType.TraitExcluder => new SkillExcluder(),
            _ => throw new NotImplementedException($"Action not implemented: {actionType}")
        };
    }
}

public static class ChampionCondtionFactory
{
    public static IChampionCondition CreateChecker(SkillConditionData data, ChampionStatData useChamp)
    {
        return data.ConditionType switch
        {
            ConditionType.None => new NullChecker(),
            ConditionType.Threshold => new StatThresholdChecker(data.StatType, data.Threshold),
            ConditionType.Compare => new StatComparisonChecker(data.StatType, useChamp),
            _ => throw new NotImplementedException($"Action not implemented: {data.ConditionType}")
        };
    }

    public static IChampionCondition CreateCondition(SkillConditionData data, ChampionStatus status)
    {
        return data.ConditionType switch
        {
            ConditionType.None => new NullChecker(),
            ConditionType.Threshold => new StatThresholdChecker(data.StatType, data.Threshold),
            ConditionType.Compare => new StatComparisonChecker(data.StatType, status.Stat),
            ConditionType.Trait => new TraitCondition(null, TraitType.None),
            _ => throw new NotImplementedException($"Action not implemented: {data.ConditionType}")
        };
    }
}
