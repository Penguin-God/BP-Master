

public class MasteryApplier
{
    public void ApplyMastery(ProGamer gamer, Champion champion)
    {
        int level = gamer.GetMastery(champion.Id);

        var newStat = champion.StatData
            .ChangeAttack(champion.StatData.Attack + level)
            .ChangeDefense(champion.StatData.Defense + level);

        champion.ChangeStat(newStat);
    }
}
