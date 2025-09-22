
public enum TraitType
{
    None,
    AttackChanger,
    DefenseChanger,
    SpeedChanger,
}


public interface ITraitAction
{
    public void Do(Champion target);
}

public class AttackChanger : ITraitAction
{
    readonly int Amount;
    public AttackChanger(int amount) => Amount = amount;
    public void Do(Champion target) => target.ChangeStat(target.StatData.ChangeAttack(target.StatData.Attack + Amount));
}

public class DefenseChanger : ITraitAction
{
    readonly int Amount;
    public DefenseChanger(int amount) => Amount = amount;
    public void Do(Champion target) => target.ChangeStat(target.StatData.ChangeDefense(target.StatData.Defense + Amount));
}

public class SpeedChanger : ITraitAction
{
    readonly int Amount;
    public SpeedChanger(int amount) => Amount = amount;
    public void Do(Champion target) => target.ChangeStat(target.StatData.ChangeSpeed(target.StatData.Speed + Amount));
}