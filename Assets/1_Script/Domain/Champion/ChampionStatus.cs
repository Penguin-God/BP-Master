

public class ChampionStatus
{
    public ChampionStatData Stat;
    public bool IsUseTrait = false;
    public ChampionStatus(ChampionStatData statData)
    {
        Stat = statData;
    }

    public void UseTrait() => IsUseTrait = true;
    public void ChangeStat(ChampionStatData newStat) => Stat = newStat;
}
