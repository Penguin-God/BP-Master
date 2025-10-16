using System;

public class ChampionStatus
{
    public ChampionStatData Stat;
    public event Action<ChampionStatData, ChampionStatData> OnStatChanged;
    public bool IsTraitExcluded { get; private set; }
    public void TraitExcluded() => IsTraitExcluded = true;
    public ChampionStatus(ChampionStatData statData) => Stat = statData;

    public void ChangeStat(ChampionStatData newStat)
    {
        if(newStat.Equals(Stat)) return;
        OnStatChanged?.Invoke(Stat, newStat);
        Stat = newStat;
    }
}
