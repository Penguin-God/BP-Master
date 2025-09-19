

public class MasteryApplier
{
    public bool ApplyMastery(ProGamer gamer, Champion champion)
    {
        int level = gamer.GetMastery(champion.Id);
        if (level == 0) return false;

        var newStat = champion.StatData
            .ChangeAttack(champion.StatData.Attack + level)
            .ChangeDefense(champion.StatData.Defense + level);

        champion.ChangeStat(newStat);
        return true;
    }
}
