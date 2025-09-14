using System;

public interface ITraitCondition
{
    bool Condition(ChampionStatData stat);
}

public class DefenseBelowCondition : ITraitCondition
{
    readonly int defenseThreshold;
    public DefenseBelowCondition(int defenseThreshold) => this.defenseThreshold = defenseThreshold;
    public bool Condition(ChampionStatData stat) => stat.Defense < defenseThreshold;
}

public enum TraitConditionType
{
    None,
    DefenseBelow, // 이하
    DefenseAtLeast, // 이상
}


public class TraitConditionChecker
{
    public bool CheckCondition(TraitConditionType type, ChampionStatData data, int threshold)
    {
        return type switch
        {
            TraitConditionType.DefenseBelow => data.Defense <= threshold,
            _ => throw new NotImplementedException($"Condition not implemented: {type}")
        };
    }
}
