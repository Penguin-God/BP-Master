using System;

public enum TraitConditionType
{
    None,

    DefenseBelow, // 이상
    DefenseAtLeast, // 이하

    AttackBelow,
    AttackAtLeast,

    SpeedBelow,
    SpeedAtLeast,
}

public interface ITraitConditionChecker
{
    public bool Check(ChampionStatData stat);
}

public class StatThresholdChecker : ITraitConditionChecker
{
    readonly TraitConditionType Type;
    readonly int Threshold;
    public StatThresholdChecker(TraitConditionType type, int threshold)
    {
        Type = type;
        Threshold = threshold;
    }

    public bool Check(ChampionStatData stat)
    {
        return Type switch
        {
            TraitConditionType.None => true,

            TraitConditionType.DefenseBelow => stat.Defense <= Threshold,
            TraitConditionType.DefenseAtLeast => stat.Defense >= Threshold,

            TraitConditionType.AttackBelow => stat.Attack <= Threshold,
            TraitConditionType.AttackAtLeast => stat.Attack >= Threshold,

            TraitConditionType.SpeedBelow => stat.Speed <= Threshold,
            TraitConditionType.SpeedAtLeast => stat.Speed >= Threshold,

            _ => throw new NotImplementedException($"Condition not implemented: {Type}")
        };
    }
}

public class TraitConditionChecker
{
    public bool CheckCondition(TraitConditionData conditionData, ChampionStatData useChampStat, ChampionStatData targetStat)
    {
        if (conditionData.IsCompareOppnent) return CompareTarget(conditionData.ConditionType, useChampStat, targetStat);
        else return new StatThresholdChecker(conditionData.ConditionType, conditionData.Threshold).Check(targetStat);
    }

    bool CompareTarget(TraitConditionType type, ChampionStatData useChampStat, ChampionStatData targetStat)
    {
        return type switch
        {
            TraitConditionType.None => true,

            TraitConditionType.DefenseBelow => useChampStat.Defense <= targetStat.Defense,
            TraitConditionType.DefenseAtLeast => useChampStat.Defense >= targetStat.Defense,

            TraitConditionType.AttackBelow => useChampStat.Attack <= targetStat.Attack,
            TraitConditionType.AttackAtLeast => useChampStat.Attack >= targetStat.Attack,

            TraitConditionType.SpeedBelow => useChampStat.Speed <= targetStat.Speed,
            TraitConditionType.SpeedAtLeast => useChampStat.Speed >= targetStat.Speed,

            _ => throw new NotImplementedException($"Condition not implemented: {type}")
        };
    }
}