
public class MasteryApplier
{
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
}
