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

public class TraitConditionChecker
{
    public bool CheckCondition(TraitConditionData conditionData, ChampionStatData stat)
    {
        return conditionData.ConditionType switch
        {
            TraitConditionType.None => true,

            TraitConditionType.DefenseBelow => stat.Defense <= conditionData.Threshold,
            TraitConditionType.DefenseAtLeast => stat.Defense >= conditionData.Threshold,

            TraitConditionType.AttackBelow => stat.Attack <= conditionData.Threshold,
            TraitConditionType.AttackAtLeast => stat.Attack >= conditionData.Threshold,

            TraitConditionType.SpeedBelow => stat.Speed <= conditionData.Threshold,
            TraitConditionType.SpeedAtLeast => stat.Speed >= conditionData.Threshold,

            _ => throw new NotImplementedException($"Condition not implemented: {conditionData.ConditionType}")
        };
    }

    public bool CheckCondition(TraitConditionData conditionData, ChampionStatData useChampStat, ChampionStatData targetStat)
    {
        return conditionData.ConditionType switch
        {
            TraitConditionType.None => true,

            TraitConditionType.DefenseBelow => useChampStat.Defense <= targetStat.Defense,
            TraitConditionType.DefenseAtLeast => useChampStat.Defense >= targetStat.Defense,

            TraitConditionType.AttackBelow => useChampStat.Attack <= targetStat.Attack,
            TraitConditionType.AttackAtLeast => useChampStat.Attack >= targetStat.Attack,

            TraitConditionType.SpeedBelow => useChampStat.Speed <= targetStat.Speed,
            TraitConditionType.SpeedAtLeast => useChampStat.Speed >= targetStat.Speed,

            _ => throw new NotImplementedException($"Condition not implemented: {conditionData.ConditionType}")
        };
    }
}