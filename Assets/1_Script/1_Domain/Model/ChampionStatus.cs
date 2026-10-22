using System;

public class ChampionStatus
{
    public ChampionStatData Stat;
    public event Action<ChampionStatData, ChampionStatData> OnStatChanged;

    public ChampionStatus(ChampionStatData statData)
    {
        Stat = statData;
    }

    public ChampionStatus DeepCopy() => new ChampionStatus(Stat);

    public void AddAttack(int att) => ChangeStat(Stat.ChangeAttack(att + Stat.Attack));
    public void AddDefense(int def) => ChangeStat(Stat.ChangeDefense(def + Stat.Defense));
    public void AddSpeed(int speed) => ChangeStat(Stat.ChangeSpeed(speed + Stat.Speed));

    public void ChangeStat(ChampionStatData newStat)
    {
        if (newStat.Equals(Stat)) return;

        OnStatChanged?.Invoke(Stat, newStat);
        Stat = newStat;
    }

    public void ChangeStat(int changeAmount, StatType statType) => ChangeStat(GetChangeStat(changeAmount, statType));

    public int GetStatAmount(StatType statType) => statType switch
    {
        StatType.Attack => Stat.Attack,
        StatType.Defense => Stat.Defense,
        StatType.Speed => Stat.Speed,
        _ => 0
    };

    ChampionStatData GetChangeStat(int amount, StatType statType) => statType switch
    {
        StatType.Attack => Stat.ChangeAttack(Stat.Attack + amount),
        StatType.Defense => Stat.ChangeDefense(Stat.Defense + amount),
        StatType.Speed => Stat.ChangeSpeed(Stat.Speed + amount),
        _ => Stat
    };
}
