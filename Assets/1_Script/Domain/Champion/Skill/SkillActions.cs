using System;

public interface ISkillAction
{
    void Do(ChampionStatus target);
}

public class AttackChanger : ISkillAction
{
    readonly int Amount;
    public AttackChanger(int amount) => Amount = amount;
    public void Do(ChampionStatus target) => target.AddAttackWithRate(Amount);
}

public class DefenseChanger : ISkillAction
{
    readonly int Amount;
    public DefenseChanger(int amount) => Amount = amount;

    readonly ISkillAmountCalculator AmountCalculator;
    public DefenseChanger(ISkillAmountCalculator amountCalculator) => AmountCalculator = amountCalculator;
    public void Do2(ChampionStatus target) => target.ChangeStatWithRate(target.Stat.ChangeDefense(target.Stat.Defense + Amount));

    public void Do(ChampionStatus target) => target.AddDefenseWithRate(AmountCalculator.Calculate(target.Stat.Defense));
}

public class SpeedChanger : ISkillAction
{
    readonly int Amount;
    public SpeedChanger(int amount) => Amount = amount;
    public void Do(ChampionStatus target) => target.ChangeStatWithRate(target.Stat.ChangeSpeed(target.Stat.Speed + Amount));
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

public class DefenseAbsorber : ISkillAction
{
    readonly ChampionStatus User;
    readonly float Percent;
    readonly ISkillAmountCalculator AmountCalculator;
    public DefenseAbsorber(ChampionStatus user, ISkillAmountCalculator amountCalculator)
    {
        User = user;
        AmountCalculator = amountCalculator;
    }

    public DefenseAbsorber(ChampionStatus user, float percent)
    {
        User = user;
        Percent = percent;
    }

    public void Do(ChampionStatus target)
    {
        int amount = (int)Math.Round(target.Stat.Defense * Percent, MidpointRounding.AwayFromZero);
        new DefenseChanger(amount).Do2(User);
        new DefenseChanger(amount * -1).Do2(target);
    }
}

public class Resonance : ISkillAction
{
    readonly ChampionStatus User;
    readonly float Percent;
    public Resonance(ChampionStatus user, float percent)
    {
        User = user;
        Percent = percent;
    }

    public void Do(ChampionStatus target)
    {
        if (target == User) return; // 자신은 제외
        int attAmount = (int)Math.Round(User.Stat.Attack * Percent, MidpointRounding.AwayFromZero);
        new AttackChanger(attAmount).Do(target);

        int defAmount = (int)Math.Round(User.Stat.Defense * Percent, MidpointRounding.AwayFromZero);
        new DefenseChanger(attAmount).Do2(target);
    }
}


public class AmplifyChanger : ISkillAction
{
    readonly float amount;

    public AmplifyChanger(float amount) => this.amount = amount;

    public void Do(ChampionStatus target) => target.AddUpRate(amount);
}