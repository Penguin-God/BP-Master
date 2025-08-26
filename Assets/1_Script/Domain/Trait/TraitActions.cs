public interface ITraitAction
{
    public ChampionStatData Do(ChampionStatData stat);
}

public class AttackChanger : ITraitAction
{
    readonly int Amount;
    public AttackChanger(int amount) => Amount = amount;

    public ChampionStatData Do(ChampionStatData stat) => stat.ChangeAttack(stat.Attack + Amount);
}
