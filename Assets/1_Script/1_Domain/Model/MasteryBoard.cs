using System;

public class MasteryBoard
{
    public int AttackLevel { get; protected set; }
    public int DefenseLevel { get; protected set; }
    public int SpeedLevel { get; protected set; }

    const int MaxLevel = 1;

    public MasteryBoard(int attackLevel = 0, int defenseLevel = 0, int speedLevel = 0)
    {
        AttackLevel = attackLevel;
        DefenseLevel = defenseLevel;
        SpeedLevel = speedLevel;
    }

    public bool Upgrade(StatType statType) => ChangeValue(statType, 1);
    public bool Downgrade(StatType statType) => ChangeValue(statType, -1);

    bool ChangeValue(StatType statType, int delta)
    {
        int statLevel = GetLevel(statType);
        int next = CheckValue(statLevel + delta);

        if (statLevel == next) return false;

        switch (statType)
        {
            case StatType.Attack: AttackLevel = next; break;
            case StatType.Defense: DefenseLevel = next; break;
            case StatType.Speed: SpeedLevel = next; break;
        }
        return true;
    }

    int GetLevel(StatType statType) => statType switch
    {
        StatType.Attack => AttackLevel,
        StatType.Defense => DefenseLevel,
        StatType.Speed => SpeedLevel,
        _ => throw new ArgumentOutOfRangeException(nameof(statType))
    };

    int CheckValue(int newValue)
    {
        if (newValue >= MaxLevel) return MaxLevel;
        else if (newValue < 0) return 0;
        else return newValue;
    }
}