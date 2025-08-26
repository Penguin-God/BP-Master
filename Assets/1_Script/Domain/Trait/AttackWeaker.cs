public class AttackWeaker : ITraitAction
{
    readonly int Amount;
    public AttackWeaker(int amount) => Amount = amount;

    public ChampionStatData Do(ChampionStatData stat) => stat.ChangeAttack(stat.Attack - Amount);
}
