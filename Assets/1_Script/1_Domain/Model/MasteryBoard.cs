using System;

public class MasteryBoard
{
    public int AttackLevel { get; protected set; }
    public int DefenseLevel { get; protected set; }
    public int SpeedLevel { get; protected set; }

    const int MaxLevel = 1;

    public void Upgrade(StatType statType)
    {
        switch (statType)
        {
            case StatType.Attack:
                EnsureCanUpgrade(AttackLevel);
                AttackLevel++;
                break;
            case StatType.Defense:
                EnsureCanUpgrade(DefenseLevel);
                DefenseLevel++;
                break;
            case StatType.Speed:
                EnsureCanUpgrade(SpeedLevel);
                SpeedLevel++;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(statType));
        }
    }

    void EnsureCanUpgrade(int currentLevel)
    {
        if (currentLevel >= MaxLevel)
            throw new InvalidOperationException($"이미 최대 레벨({MaxLevel})에 도달했습니다.");
    }
}