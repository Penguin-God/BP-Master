using System;

public enum StatConditionType
{
    None,

    DefenseBelow,
    DefenseAtLeast,

    AttackBelow,
    AttackAtLeast,

    SpeedBelow,
    SpeedAtLeast,
}

public interface IChampionCondition
{
    public bool Check(ChampionStatus target);
}

public class NullChecker : IChampionCondition
{
    public bool Check(ChampionStatus status) => true;
}

public class StatThresholdChecker : IChampionCondition // 이상, 이하
{
    readonly StatConditionType Type;
    readonly int Threshold;
    public StatThresholdChecker(StatConditionType type, int threshold)
    {
        Type = type;
        Threshold = threshold;
    }

    public bool Check(ChampionStatus target)
    {
        var stat = target.Stat;
        return Type switch
        {
            StatConditionType.None => true,

            StatConditionType.DefenseBelow => stat.Defense <= Threshold,
            StatConditionType.DefenseAtLeast => stat.Defense >= Threshold,

            StatConditionType.AttackBelow => stat.Attack <= Threshold,
            StatConditionType.AttackAtLeast => stat.Attack >= Threshold,

            StatConditionType.SpeedBelow => stat.Speed <= Threshold,
            StatConditionType.SpeedAtLeast => stat.Speed >= Threshold,

            _ => throw new NotImplementedException($"Condition not implemented: {Type}")
        };
    }
}

public class StatComparisonChecker : IChampionCondition // 초과, 미만
{
    readonly StatConditionType Type;
    readonly ChampionStatData UseChamp;
    public StatComparisonChecker(StatConditionType type, ChampionStatData useChamp)
    {
        Type = type;
        UseChamp = useChamp;
    }

    public bool Check(ChampionStatus target)
    {
        var targetStat = target.Stat;
        return Type switch
        {
            StatConditionType.None => true,

            StatConditionType.DefenseAtLeast => UseChamp.Defense < targetStat.Defense,
            StatConditionType.DefenseBelow => UseChamp.Defense > targetStat.Defense,

            StatConditionType.AttackAtLeast => UseChamp.Attack < targetStat.Attack,
            StatConditionType.AttackBelow => UseChamp.Attack > targetStat.Attack,

            StatConditionType.SpeedAtLeast => UseChamp.Speed < targetStat.Speed,
            StatConditionType.SpeedBelow => UseChamp.Speed > targetStat.Speed,
            
            _ => throw new NotImplementedException($"Condition not implemented: {Type}")
        };
    }
}