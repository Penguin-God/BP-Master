using System;

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

    public static IChampionCondition CreateNull() => new NullChecker();
    public static IChampionCondition CreateThreshold(StatConditionType compareType, int threshold) => new StatThresholdChecker(compareType, threshold);
    public static IChampionCondition CreateCompare(StatConditionType compareType, ChampionStatData useChamp) => new StatComparisonChecker(compareType, useChamp);
}