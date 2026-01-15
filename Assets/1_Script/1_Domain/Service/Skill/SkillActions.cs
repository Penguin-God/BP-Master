using System;

public interface ISkillAction
{
    void Do(ChampionStatus target);
}

public enum StatType
{
    Attack,
    Defense,
    Speed,
}

public class StatChanger : ISkillAction
{
    private readonly StatType StatType;
    private readonly ISkillAmountCalculator Calculator;

    public StatChanger(StatType statType, ISkillAmountCalculator calculator)
    {
        StatType = statType;
        Calculator = calculator;
    }

    public void Do(ChampionStatus target)
    {
        var stat = target.Stat;

        int baseValue = StatType switch
        {
            StatType.Attack => stat.Attack,
            StatType.Defense => stat.Defense,
            StatType.Speed => stat.Speed,
            _ => 0
        };

        int amount = Calculator.Calculate(baseValue);

        target.ChangeStat(CreateNewStat(amount, stat));
    }

    ChampionStatData CreateNewStat(int amount, ChampionStatData stat) => StatType switch
    {
        StatType.Attack => new ChampionStatData(stat.Attack + amount, stat.Defense, stat.Speed),
        StatType.Defense => new ChampionStatData(stat.Attack, stat.Defense + amount, stat.Speed),
        StatType.Speed => new ChampionStatData(stat.Attack, stat.Defense, stat.Speed + amount),
        _ => stat
    };
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

public class PickChampBuffer : ISkillAction
{
    readonly PhaseActionEventDispatcher eventDispatcher;
    readonly int Amount;
    public PickChampBuffer(PhaseActionEventDispatcher eventDispatcher, int amount)
    {
        this.eventDispatcher = eventDispatcher;
        this.Amount = amount;
    }

    public void Do(ChampionStatus target)
    {
        eventDispatcher.OnChampionPick += (champ) => champ.Status.AddAttackWithRate(Amount);
    }
}