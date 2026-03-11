
public class MasteryApplier
{
    readonly MasteryCollection masteryCollection;
    public MasteryApplier(MasteryCollection masteryCollection)
    {
        this.masteryCollection = masteryCollection;
    }

    //public void ApplyMastery(int id, ChampionStatus status)
    //{
    //    int level = masteryCollection.GetMasteryLevel(id);
    //    var oldStat = status.Stat;
    //    var newStat = new ChampionStatData(
    //        oldStat.Attack + level,
    //        oldStat.Defense + level,
    //        oldStat.Speed
    //    );
    //    status.ChangeStat(newStat);
    //}

    public void ApplyMastery(int id, ChampionStatus status)
    {
        var masteryStat = masteryCollection.GetMasteryStat(id);
        var newStat = status.Stat + masteryStat;
        status.ChangeStat(newStat);
    }
}
