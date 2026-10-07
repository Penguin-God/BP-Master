using System;

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