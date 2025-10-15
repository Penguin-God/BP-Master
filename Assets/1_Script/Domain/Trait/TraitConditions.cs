using System;

public enum TraitConditionType
{
    None,

    DefenseBelow, // 이상
    DefenseAtLeast, // 이하

    AttackBelow,
    AttackAtLeast,

    SpeedBelow,
    SpeedAtLeast
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
}