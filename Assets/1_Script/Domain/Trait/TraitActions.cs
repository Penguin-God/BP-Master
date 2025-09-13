public interface ITraitAction
{
    public ChampionStatData Do(ChampionStatData stat);
}

public class AttackChanger : ITraitAction
{
    readonly Champion Target;
    readonly int Amount;
    public AttackChanger(int amount) => Amount = amount;
    public AttackChanger(Champion target, int amount)
    {
        Target = target;
        Amount = amount;
    }

    public ChampionStatData Do(ChampionStatData stat) => stat.ChangeAttack(stat.Attack + Amount);

    public void Do() => Target.ChangeStat(Target.StatData.ChangeAttack(Target.StatData.Attack + Amount));
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