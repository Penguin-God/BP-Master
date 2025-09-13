public interface ITraitAction
{
    public ChampionStatData Do(ChampionStatData stat);
}

public class AttackChanger : ITraitAction
{
    readonly int Amount;
    public AttackChanger(int amount) => Amount = amount;

    public ChampionStatData Do(ChampionStatData stat) => stat.ChangeAttack(stat.Attack + Amount);

    public void Do(Champion target) => target.ChangeStat(target.StatData.ChangeAttack(target.StatData.Attack + Amount));
}


public class DefenseChanger : ITraitAction
{
    readonly int Amount;
    public DefenseChanger(int amount) => Amount = amount;

    public ChampionStatData Do(ChampionStatData stat) => stat.ChangeDefense(stat.Defense + Amount);
}

public class SpeedChanger : ITraitAction
{
    readonly int Amount;
    public SpeedChanger(int amount) => Amount = amount;

    public ChampionStatData Do(ChampionStatData stat) => stat.ChangeSpeed(stat.Speed + Amount);
}