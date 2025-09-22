

public class ChampionStatus
{
    public ChampionStatData StatData;
    public bool IsUseTrait = false;
    public ChampionStatus(ChampionStatData statData)
    {
        StatData = statData;
    }

    public void UseTrait() => IsUseTrait = true;
    public void ChangeStat(ChampionStatData newStat) => StatData = newStat;
}
