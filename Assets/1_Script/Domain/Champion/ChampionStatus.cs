using System;

public class ChampionStatus
{
    public ChampionStatData Stat;
    public event Action<ChampionStatData, ChampionStatData> OnStatChanged;

    public bool IsTraitExcluded { get; private set; }
    public void TraitExcluded() => IsTraitExcluded = true;

    float UpRate= 1f;
    float DownRate= 1f;

    public ChampionStatus(ChampionStatData statData) => Stat = statData;

    public readonly TraitType TraitType;
    public ChampionStatus(ChampionStatData statData, TraitType traitType)
    {
        Stat = statData;
        TraitType = traitType;
    }

    public void AddUpRate(float upRate) => UpRate += upRate;
    public void AddDownRate(float downRate) => DownRate += downRate;


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
