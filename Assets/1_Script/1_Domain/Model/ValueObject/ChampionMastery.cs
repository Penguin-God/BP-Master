public readonly struct ChampionMastery
{
    public readonly int ChampionId;
    public readonly int Level;
    public readonly ChampionStatData MasteryStat;
    public ChampionMastery(int championId, int level)
    {
        ChampionId = championId;
        Level = level;
        MasteryStat = new ChampionStatData(level, level, 0);
    }
}