using System;

public enum TraitConditionType
{
    None,

    DefenseBelow,
    DefenseAtLeast,

    AttackBelow,
    AttackAtLeast,

    SpeedBelow,
    SpeedAtLeast,
}

public interface ITraitConditionChecker
{
    public bool Check(ChampionStatData stat);
}

public class NullChecker : ITraitConditionChecker
{
    public bool Check(ChampionStatData stat) => true;
}

public class StatThresholdChecker : ITraitConditionChecker // 이상, 이하
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

public class StatComparisonChecker : ITraitConditionChecker // 초과, 미만
{
    readonly TraitConditionType Type;
    readonly ChampionStatData UseChamp;
    public StatComparisonChecker(TraitConditionType type, ChampionStatData useChamp)
    {
        Type = type;
        UseChamp = useChamp;
    }

    public bool Check(ChampionStatData target)
    {
        return Type switch
        {
            TraitConditionType.None => true,

            TraitConditionType.DefenseBelow => UseChamp.Defense < target.Defense,
            TraitConditionType.DefenseAtLeast => UseChamp.Defense > target.Defense,

            TraitConditionType.AttackBelow => UseChamp.Attack < target.Attack,
            TraitConditionType.AttackAtLeast => UseChamp.Attack > target.Attack,

            TraitConditionType.SpeedBelow => UseChamp.Speed < target.Speed,
            TraitConditionType.SpeedAtLeast => UseChamp.Speed > target.Speed,

            _ => throw new NotImplementedException($"Condition not implemented: {Type}")
        };
    }
}