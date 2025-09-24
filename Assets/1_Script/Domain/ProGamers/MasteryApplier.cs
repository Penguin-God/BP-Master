
public class MasteryApplier
{
    public void ApplyMastery(ChampionStatus status, int level)
    {
        var newStat = new MasteryCalculator().ApplyMastery(status.Stat, level);
        status.ChangeStat(newStat);
    }
}

public class MasteryCalculator
{
    public ChampionStatData ApplyMastery(ChampionStatData stat, int level)
    {
        if (level <= 0) return stat;

        return new ChampionStatData(stat.Attack + level, stat.Defense + level, stat.Speed);
    }
}
