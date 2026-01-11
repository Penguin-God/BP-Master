
public class MasteryApplier
{
    readonly MasteryCollection masteryCollection;
    public MasteryApplier(MasteryCollection masteryCollection)
    {
        this.masteryCollection = masteryCollection;
    }

    public void ApplyStatChange(ChampionStatus status, int masteryLevel)
    {
        var oldStat = status.Stat;
        var newStat = new ChampionStatData(
            oldStat.Attack + masteryLevel,
            oldStat.Defense + masteryLevel,
            oldStat.Speed
        );
        status.ChangeStat(newStat);
    }

    public void ApplyMastery(int id, ChampionStatus status)
    {
        int level = masteryCollection.GetMasteryLevel(id);
        var oldStat = status.Stat;
        var newStat = new ChampionStatData(
            oldStat.Attack + level,
            oldStat.Defense + level,
            oldStat.Speed
        );
        status.ChangeStat(newStat);
    }
}
