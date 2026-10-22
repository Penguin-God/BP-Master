using System;

public class NullChecker : IChampionCondition
{
    public bool Check(ChampionStatus status, ChampionStatus caster) => true;
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

    public bool Check(ChampionStatus target, ChampionStatus caster)
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
    public StatComparisonChecker(StatConditionType type) => Type = type;

    public bool Check(ChampionStatus target, ChampionStatus caster)
    {
        var targetStat = target.Stat;
        var casterStat = caster.Stat;
        return Type switch
        {
            StatConditionType.None => true,

            StatConditionType.DefenseAtLeast => casterStat.Defense < targetStat.Defense,
            StatConditionType.DefenseBelow => casterStat.Defense > targetStat.Defense,

            StatConditionType.AttackAtLeast => casterStat.Attack < targetStat.Attack,
            StatConditionType.AttackBelow => casterStat.Attack > targetStat.Attack,

            StatConditionType.SpeedAtLeast => casterStat.Speed < targetStat.Speed,
            StatConditionType.SpeedBelow => casterStat.Speed > targetStat.Speed,
            
            _ => throw new NotImplementedException($"Condition not implemented: {Type}")
        };
    }
}