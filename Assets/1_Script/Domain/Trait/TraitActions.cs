
public enum TraitType
{
    None,
    AttackChanger,
    DefenseChanger,
    SpeedChanger,
}

public interface ITraitAction
{
    void Do(ChampionStatus target);
}

public class AttackChanger : ITraitAction
{
    readonly int Amount;
    public AttackChanger(int amount) => Amount = amount;
    public void Do(ChampionStatus target) => target.ChangeStat(target.Stat.ChangeAttack(target.Stat.Attack + Amount));
}

public class DefenseChanger : ITraitAction
{
    readonly int Amount;
    public DefenseChanger(int amount) => Amount = amount;
    public void Do(ChampionStatus target) => target.ChangeStat(target.Stat.ChangeDefense(target.Stat.Defense + Amount));
}

public class SpeedChanger : ITraitAction
{
    readonly int Amount;
    public SpeedChanger(int amount) => Amount = amount;
    public void Do(ChampionStatus target) => target.ChangeStat(target.Stat.ChangeSpeed(target.Stat.Speed + Amount));
}
