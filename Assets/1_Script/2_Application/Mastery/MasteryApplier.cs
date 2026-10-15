public class MasteryApplier
{
    readonly MasteryStatCollection _statProvider;

    public MasteryApplier(MasteryStatCollection statProvider) => _statProvider = statProvider;

    public ChampionStatData GetBonusStat(int id) => _statProvider.GetMasteryStat(id);

    public void ApplyMastery(int id, ChampionStatus status)
    {
        var newStat = status.Stat + GetBonusStat(id);
        status.ChangeStat(newStat);
    }
}