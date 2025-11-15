using System;

public interface ISkillAction
{
    void Do(ChampionStatus target);
}

public class AttackChanger : ISkillAction
{
    readonly int Amount;
    public AttackChanger(int amount) => Amount = amount;
    public void Do(ChampionStatus target) => target.ChangeStatWithRate(target.Stat.ChangeAttack(target.Stat.Attack + Amount));
}

public class DefenseChanger : ISkillAction
{
    readonly int Amount;
    public DefenseChanger(int amount) => Amount = amount;
    public void Do(ChampionStatus target) => target.ChangeStatWithRate(target.Stat.ChangeDefense(target.Stat.Defense + Amount));
}

public class SpeedChanger : ISkillAction
{
    readonly int Amount;
    public SpeedChanger(int amount) => Amount = amount;
    public void Do(ChampionStatus target) => target.ChangeStatWithRate(target.Stat.ChangeSpeed(target.Stat.Speed + Amount));
}


public class DefenseFixer : ISkillAction
{
    readonly int Value;
    public DefenseFixer(int value) => this.Value = value;

    public void Do(ChampionStatus target) => target.ChangeStat(new ChampionStatData(target.Stat.Attack, Value, target.Stat.Speed));
}

public class SkillExcluder : ISkillAction
{
    public void Do(ChampionStatus target) => target.TraitExcluded();
}

public class AttackPercentChanger : ISkillAction
{
    readonly float Percent;
    public AttackPercentChanger(float percent) => Percent = percent;

    public void Do(ChampionStatus target)
    {
        int amount = (int)Math.Round(target.Stat.Attack * Percent, MidpointRounding.AwayFromZero);
        new AttackChanger(amount).Do(target);
    }
}

public class DefensePercentChanger : ISkillAction
{
    readonly float Percent;
    public DefensePercentChanger(float percent) => Percent = percent;

    public void Do(ChampionStatus target)
    {
        int amount = (int)Math.Round(target.Stat.Defense * Percent, MidpointRounding.AwayFromZero);
        new DefenseChanger(amount).Do(target);
    }
}

public class DefenseAbsorber : ISkillAction
{
    readonly ChampionStatus User;
    readonly float Percent;
    public DefenseAbsorber(ChampionStatus user, float percent)
    {
        User = user;
        Percent = percent;
    }

    public void Do(ChampionStatus target)
    {

    }
}