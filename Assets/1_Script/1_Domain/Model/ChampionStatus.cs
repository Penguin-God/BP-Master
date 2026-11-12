using System;

public class ChampionStatus
{
    public ChampionStatData Stat;
    public event Action<ChampionStatData, ChampionStatData> OnStatChanged;
    public bool IsDead => Stat.Defense <= 0;

    public ChampionStatus(ChampionStatData statData)
    {
        Stat = statData;
    }

    public ChampionStatus DeepCopy() => new ChampionStatus(Stat);

    public void Die() => ChangeStat(new ChampionStatData(0, 0));

    public void TakeDamage(int damage)
    {
        int nextDefense = Stat.Defense - damage;
        if (nextDefense <= 0) Die();
        else ChangeStat(Stat.ChangeDefense(nextDefense));
    }

    public void AddAttack(int att) => ChangeStat(Stat.ChangeAttack(att + Stat.Attack));

    public void AddDefense(int def)
    {
        int nextDefense = Stat.Defense + def;
        if (nextDefense <= 0) Die();
        else ChangeStat(Stat.ChangeDefense(nextDefense));
    }

    public void ChangeStat(ChampionStatData newStat)
    {
        if (Stat.Defense > 0 && newStat.Defense <= 0) newStat = new ChampionStatData(0, 0);
        if (newStat.Equals(Stat)) return;

        OnStatChanged?.Invoke(Stat, newStat);
        Stat = newStat;
    }

    public void AddStat(int changeAmount, StatType statType) => ChangeStat(GetChangeStat(changeAmount, statType));

    public int GetStatAmount(StatType statType) => statType switch
    {
        StatType.Attack => Stat.Attack,
        StatType.Defense => Stat.Defense,
        _ => 0
    };

    ChampionStatData GetChangeStat(int amount, StatType statType) => statType switch
    {
        StatType.Attack => Stat.ChangeAttack(Stat.Attack + amount),
        StatType.Defense => Stat.Defense + amount <= 0 ? new ChampionStatData(0, 0) : Stat.ChangeDefense(Stat.Defense + amount),
        _ => Stat
    };
}
