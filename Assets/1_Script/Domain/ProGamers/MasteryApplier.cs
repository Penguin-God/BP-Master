

public class MasteryApplier
{
    public void ApplyMastery(ChampionStatus champion, int level)
    {
        if (level <= 0) return;

        var newStat = champion.StatData
            .ChangeAttack(champion.StatData.Attack + level)
            .ChangeDefense(champion.StatData.Defense + level);

        champion.ChangeStat(newStat);
    }
}
