public class MasteryApplier
{
    readonly MasteryStatCollection _statProvider;

    public MasteryApplier(MasteryStatCollection statProvider) => _statProvider = statProvider;

    public void ApplyMastery(int id, ChampionStatus status)
    {
        var masteryStat = _statProvider.GetMasteryStat(id);
        var newStat = status.Stat + masteryStat;
        status.ChangeStat(newStat);
    }
}