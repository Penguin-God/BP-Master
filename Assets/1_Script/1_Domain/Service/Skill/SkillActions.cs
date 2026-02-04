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
    readonly StatType StatType;
    readonly ISkillAmountCalculator Calculator;

    public StatChanger(StatType statType, ISkillAmountCalculator calculator)
    {
        StatType = statType;
        Calculator = calculator;
    }

    public void Do(ChampionStatus target)
    {
        int changeAmount = Calculator.Calculate(target.GetStatAmount(StatType));
        target.ChangeStat(changeAmount, StatType);
    }
}

public class SkillExcluder : ISkillAction
{
    public void Do(ChampionStatus target) => target.TraitExcluded();
}

public class DefenseAbsorber : ISkillAction
{
    readonly ChampionStatus User;
    readonly ISkillAmountCalculator AmountCalculator; // 기존 로직용
    readonly StatType StatType;

    public DefenseAbsorber(ChampionStatus user, ISkillAmountCalculator amountCalculator, StatType statType)
    {
        User = user;
        AmountCalculator = amountCalculator;
        StatType = statType;
    }

    public void Do(ChampionStatus target)
    {
        int amount = AmountCalculator.Calculate(target.GetStatAmount(StatType));
        User.ChangeStat(amount, StatType);
        target.ChangeStat(amount * -1, StatType);
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

public class PickChampStatChanger : ISkillAction
{
    readonly PhaseActionEventDispatcher eventDispatcher;
    readonly StatChanger _statChanger;
    readonly Team Team;

    public PickChampStatChanger(PhaseActionEventDispatcher eventDispatcher, StatChanger statChanger, Team team)
    {
        this.eventDispatcher = eventDispatcher;
        this._statChanger = statChanger;
        this.Team = team;
    }

    public void Do(ChampionStatus target) => eventDispatcher.OnChampionPick += ChangeStat;

    void ChangeStat(Champion champion, Team team)
    {
        if(Team == team)
            _statChanger.Do(champion.Status);
    }
}

public class Doppelganger : ISkillAction
{
    readonly ChampionStatus caster;
    public Doppelganger(ChampionStatus caster) => this.caster = caster;
    public void Do(ChampionStatus target) => caster.ChangeStat(target.Stat);
}

public class FinalStatChanger : ISkillAction
{
    readonly ChampionStatus _caster;
    readonly PhaseEventDispatcher _dispatcher;
    readonly ISkillAmountCalculator _calculator;

    public FinalStatChanger(ChampionStatus caster, PhaseEventDispatcher dispatcher, ISkillAmountCalculator calculator)
    {
        _caster = caster;
        _dispatcher = dispatcher;
        _calculator = calculator;
    }

    public void Do(ChampionStatus target)
    {
        _dispatcher.OnPhaseDone += ChangeStat;
    }

    void ChangeStat()
    {
        _dispatcher.OnPhaseDone -= ChangeStat;
        var s = _caster.Stat;
        _caster.AddAttackWithRate(_calculator.Calculate(s.Attack));
        _caster.AddDefenseWithRate(_calculator.Calculate(s.Defense));
        _caster.AddSpeedWithRate(_calculator.Calculate(s.Speed));
    }
}