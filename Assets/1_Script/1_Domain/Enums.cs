public enum CardPoolType
{
    Available,
    Selected
}

public enum StatType
{
    Attack,
    Defense,
    Speed,
}


public enum Side { Self, Opponent, All }
public enum TargetRange
{
    None,
    Single,
    Double,
    Triple,
    All,
}

public enum SkillType
{
    None = 0,
    StatChanger = 2,
    TraitExcluder = 7,
    StatAbsorber = 8,
    Resonance = 9,
    AmplifyChanger = 10,
    PickBuffer = 11,
    Doppelganger = 12,
    FinalStatChanger = 13,
}

public enum ConditionType
{
    None,
    Threshold,
    Compare,
}

public enum AmountType { None, Value, Percent, Fix }


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