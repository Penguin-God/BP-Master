public enum GamePhase { Ban, Pick, Skill, Done }
public enum Team { Blue, Red, All }
public enum StatType { Attack, Defense, Speed }

public enum Side { Self, Opponent, All, OneSelf }
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
    PickBuffer = 11,
    FinalStatChanger = 13,
    OppentPickStatChanger = 14,
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