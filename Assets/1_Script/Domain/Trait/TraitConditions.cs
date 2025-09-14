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
    public bool CheckCondition(TraitConditionType type, ChampionStatData data, int threshold)
    {
        return type switch
        {
            TraitConditionType.None => true,

            TraitConditionType.DefenseBelow => data.Defense <= threshold,
            TraitConditionType.DefenseAtLeast => data.Defense >= threshold,

            TraitConditionType.AttackBelow => data.Attack <= threshold,
            TraitConditionType.AttackAtLeast => data.Attack >= threshold,

            TraitConditionType.SpeedBelow => data.Speed <= threshold,
            TraitConditionType.SpeedAtLeast => data.Speed >= threshold,

            _ => throw new NotImplementedException($"Condition not implemented: {type}")
        };
    }
}