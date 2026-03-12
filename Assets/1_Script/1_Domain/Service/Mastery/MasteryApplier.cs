
public class MasteryApplier
{
    readonly MasteryCollection masteryCollection;
    public MasteryApplier(MasteryCollection masteryCollection)
    {
        this.masteryCollection = masteryCollection;
    }

    public void ApplyMastery(int id, ChampionStatus status)
    {
        var masteryStat = masteryCollection.GetMasteryStat(id);
        var newStat = status.Stat + masteryStat;
        status.ChangeStat(newStat);
    }
}
