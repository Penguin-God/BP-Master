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
    public bool Check(ChampionStatData stat);
}

public class NullChecker : IChampionCondition
{
    public bool Check(ChampionStatData stat) => true;
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

    public bool Check(ChampionStatData stat)
    {
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

    public bool Check(ChampionStatData target)
    {
        return Type switch
        {
            StatConditionType.None => true,

            StatConditionType.DefenseAtLeast => UseChamp.Defense < target.Defense,
            StatConditionType.DefenseBelow => UseChamp.Defense > target.Defense,

            StatConditionType.AttackAtLeast => UseChamp.Attack < target.Attack,
            StatConditionType.AttackBelow => UseChamp.Attack > target.Attack,

            StatConditionType.SpeedAtLeast => UseChamp.Speed < target.Speed,
            StatConditionType.SpeedBelow => UseChamp.Speed > target.Speed,
            
            _ => throw new NotImplementedException($"Condition not implemented: {Type}")
        };
    }
}

public class TraitCondition : IChampionCondition
{
    readonly ChampionStatus Status;
    readonly TraitType TargetType;
    public TraitCondition(ChampionStatus status, TraitType targetType)
    {
        Status = status;
        TargetType = targetType;
    }

    public bool Check(ChampionStatData target) => Status.TraitType == TargetType;
}