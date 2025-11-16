using System;

public class SkillExecutorFactory
{
    public SkillExecutor CreateExecutor(SkillData skillData, ChampionStatus useChamp)
    {
        ISkillAction action = SkillActionFactory.CreateAction(skillData.TraitType, skillData.Amount, useChamp);
        IChampionCondition condition = SkillCondtionFactory.CreateCondition(skillData.ConditionData, useChamp.Stat);
        return new SkillExecutor(action, condition);
    }
}

public static class SkillActionFactory
{
    public static ISkillAction CreateAction(SkillType actionType, int amount, ChampionStatus useChamp)
    {
        return actionType switch
        {
            SkillType.AttackChanger => new AttackChanger(amount),
            SkillType.DefenseChanger => new DefenseChanger(amount),
            SkillType.PercentAttackChanger => new AttackPercentChanger(GetPercent(amount)),
            SkillType.PercentDefenseChanger => new DefensePercentChanger(GetPercent(amount)),
            SkillType.SpeedChanger => new SpeedChanger(amount),
            SkillType.DefenseFixer => new DefenseFixer(amount),
            SkillType.TraitExcluder => new SkillExcluder(),
            SkillType.DefenseAbsorber => new DefenseAbsorber(useChamp, GetPercent(amount)),
            SkillType.Resonance => new Resonance(useChamp, GetPercent(amount)),
            SkillType.AmplifyChanger => new AmplifyChanger(GetPercent(amount)),
            _ => throw new NotImplementedException($"Action not implemented: {actionType}")
        };

        float GetPercent(int amount) => (float)amount / 100;
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
