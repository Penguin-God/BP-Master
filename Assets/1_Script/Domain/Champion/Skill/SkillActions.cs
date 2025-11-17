using System;

public interface ISkillAction
{
    void Do(ChampionStatus target);
}

public class AttackChanger : ISkillAction
{
    readonly ISkillAmountCalculator AmountCalculator;
    public AttackChanger(ISkillAmountCalculator amountCalculator) => AmountCalculator = amountCalculator;
    public void Do(ChampionStatus target) => target.AddAttackWithRate(AmountCalculator.Calculate(target.Stat.Attack));
}

public class DefenseChanger : ISkillAction
{
    readonly ISkillAmountCalculator AmountCalculator;
    public DefenseChanger(ISkillAmountCalculator amountCalculator) => AmountCalculator = amountCalculator;
    public void Do(ChampionStatus target) => target.AddDefenseWithRate(AmountCalculator.Calculate(target.Stat.Defense));
}

public class SpeedChanger : ISkillAction
{
    readonly ISkillAmountCalculator AmountCalculator;
    public SpeedChanger(ISkillAmountCalculator amountCalculator) => AmountCalculator = amountCalculator;
    public void Do(ChampionStatus target) => target.AddSpeedWithRate(AmountCalculator.Calculate(target.Stat.Speed));
}

public class SkillExcluder : ISkillAction
{
    public void Do(ChampionStatus target) => target.TraitExcluded();
}

public class DefenseAbsorber : ISkillAction
{
    readonly ChampionStatus User;
    readonly ISkillAmountCalculator AmountCalculator;
    public DefenseAbsorber(ChampionStatus user, ISkillAmountCalculator amountCalculator)
    {
        User = user;
        AmountCalculator = amountCalculator;
    }

    public void Do(ChampionStatus target)
    {
        int amount = AmountCalculator.Calculate(target.Stat.Defense);
        User.AddDefenseWithRate(amount);
        target.AddDefenseWithRate(amount * -1);
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
        target.AddAttackWithRate(attAmount);

        int defAmount = (int)Math.Round(User.Stat.Defense * Percent, MidpointRounding.AwayFromZero);
        target.AddDefenseWithRate(defAmount);
    }
}


public class AmplifyChanger : ISkillAction
{
    readonly float amount;

    public AmplifyChanger(float amount) => this.amount = amount;

    public void Do(ChampionStatus target) => target.AddUpRate(amount);
}