

public class MasteryApplier
{
    public ChampionStatData ApplyMastery(ChampionStatData stat, int level)
    {
        if (level <= 0) return stat;

        return new ChampionStatData(stat.Attack + level, stat.Defense + level, stat.Speed);
    }
}
