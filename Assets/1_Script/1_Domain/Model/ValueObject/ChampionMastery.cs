public readonly struct ChampionMastery
{
    public readonly int ChampionId;
    public readonly ChampionStatData MasteryStat;
    public ChampionMastery(int championId, int level)
    {
        ChampionId = championId;
        MasteryStat = new ChampionStatData(level, level, 0);
    }

    public ChampionMastery(int championId, ChampionStatData masteryStat)
    {
        ChampionId = championId;
        MasteryStat = masteryStat;
    }
}