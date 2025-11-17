using System;

public class ChampionStatus
{
    public ChampionStatData Stat;
    public event Action<ChampionStatData, ChampionStatData> OnStatChanged;

    public bool IsSkillExcluded { get; private set; }
    public void TraitExcluded() => IsSkillExcluded = true;

    public float UpRate { get; private set; } = 1f;
    public float DownRate { get; private set; } = 1f;

    public readonly TraitType TraitType;
    public ChampionStatus(ChampionStatData statData, TraitType traitType) : this(statData, traitType, false, 1f, 1f) { }

    ChampionStatus(ChampionStatData statData, TraitType traitType, bool traitExcluded, float upRate, float downRate)
    {
        Stat = statData;
        TraitType = traitType;
        IsSkillExcluded = traitExcluded;
        UpRate = upRate;
        DownRate = downRate;
    }

    public void AddUpRate(float upRate) => UpRate += upRate;
    public void AddDownRate(float downRate) => DownRate += downRate;

    public ChampionStatus DeepCopy() => new ChampionStatus(Stat, TraitType, IsSkillExcluded, UpRate, DownRate);

    public void AddAttackWithRate(int att) => ChangeStatWithRate(Stat.ChangeAttack(att + Stat.Attack));
    public void AddDefenseWithRate(int def) => ChangeStatWithRate(Stat.ChangeDefense(def + Stat.Defense));
    public void ChangeStatWithRate(ChampionStatData desiredStat)
    {
        int newAttack = ApplyRate(Stat.Attack, desiredStat.Attack);
        int newDefense = ApplyRate(Stat.Defense, desiredStat.Defense);
        int newSpeed = ApplyRate(Stat.Speed, desiredStat.Speed);

        ChangeStat(new ChampionStatData(newAttack, newDefense, newSpeed));
    }

    public void ChangeStat(ChampionStatData newStat)
    {
        if (newStat.Equals(Stat)) return;

        OnStatChanged?.Invoke(Stat, newStat);
        Stat = newStat;
    }

    int ApplyRate(int current, int desired)
    {
        int delta = desired - current;
        if (delta == 0) return current;

        float rate = delta > 0 ? UpRate : DownRate;
        int scaled = RoundAwayFromZero(delta * rate);
        return current + scaled;
    }

    int RoundAwayFromZero(float value) => (int)Math.Round(value, MidpointRounding.AwayFromZero);
}
