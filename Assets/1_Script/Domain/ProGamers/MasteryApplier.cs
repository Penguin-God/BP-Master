

public class MasteryApplier
{
    public void ApplyMastery(ChampionStatus championStatus, int level)
    {
        if (level <= 0) return;

        var newStat = championStatus.StatData
            .ChangeAttack(championStatus.StatData.Attack + level)
            .ChangeDefense(championStatus.StatData.Defense + level);

        championStatus.ChangeStat(newStat);
    }
}
