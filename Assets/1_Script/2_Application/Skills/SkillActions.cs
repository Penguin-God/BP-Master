using System;

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
        // 계산
        int changeAmount = Calculator.Calculate(target.GetStatAmount(StatType));
        // 액션
        target.ChangeStat(changeAmount, StatType);
    }
}

public class StatAbsorber : ISkillAction
{
    readonly ChampionStatus User;
    readonly ISkillAmountCalculator AmountCalculator;
    readonly StatType StatType;

    public StatAbsorber(ChampionStatus user, ISkillAmountCalculator amountCalculator, StatType statType)
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
        target.AddAttack(attAmount);

        int defAmount = (int)Math.Round(User.Stat.Defense * Percent, MidpointRounding.AwayFromZero);
        target.AddDefense(defAmount);
    }
}

public class PickChampStatChanger : ISkillAction
{
    readonly BanPickEventDispatcher eventDispatcher;
    readonly StatChanger _statChanger;
    readonly Team Team;

    public PickChampStatChanger(BanPickEventDispatcher eventDispatcher, StatChanger statChanger, Team team)
    {
        this.eventDispatcher = eventDispatcher;
        this._statChanger = statChanger;
        this.Team = team;
    }

    public void Do(ChampionStatus target) => eventDispatcher.OnTeamChampionPick += ChangeStat;

    void ChangeStat(Champion champion, Team team)
    {
        if(Team == team)
            _statChanger.Do(champion.Status);
    }
}

public class OppentPickChampStatChanger : ISkillAction
{
    readonly BanPickEventDispatcher eventDispatcher;
    readonly StatChanger _statChanger;
    readonly Team Team;

    public OppentPickChampStatChanger(BanPickEventDispatcher eventDispatcher, StatChanger statChanger, Team team)
    {
        this.eventDispatcher = eventDispatcher;
        this._statChanger = statChanger;
        this.Team = team;
    }

    public void Do(ChampionStatus target) => eventDispatcher.OnTeamChampionPick += ChangeStat;

    void ChangeStat(Champion champion, Team team)
    {
        if (Team != team)
            _statChanger.Do(champion.Status);
    }
}

public class 복리 : ISkillAction
{
    readonly BanPickEventDispatcher eventDispatcher;
    readonly StatChanger _statChanger;
    readonly Team Team;
    readonly ChampionStatus _caster;
    public 복리(ChampionStatus caster, BanPickEventDispatcher eventDispatcher, StatChanger statChanger, Team team)
    {
        _caster = caster;
        this.eventDispatcher = eventDispatcher;
        this._statChanger = statChanger;
        this.Team = team;
    }

    public void Do(ChampionStatus target) => eventDispatcher.OnTeamChampionPick += ChangeStat;

    void ChangeStat(Champion champion, Team team)
    {
        if (Team != team)
            _statChanger.Do(_caster);
    }
}

public class Doppelganger : ISkillAction
{
    readonly ChampionStatus caster;
    public Doppelganger(ChampionStatus caster) => this.caster = caster;
    public void Do(ChampionStatus target) => caster.ChangeStat(target.Stat);
}

public class Hypocrisy : ISkillAction
{
    readonly ChampionStatus caster;
    public Hypocrisy(ChampionStatus caster) => this.caster = caster;
    public void Do(ChampionStatus target) => target.ChangeStat(target.Stat + caster.Stat);
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
        _caster.AddAttack(_calculator.Calculate(s.Attack));
        _caster.AddDefense(_calculator.Calculate(s.Defense));
        _caster.AddSpeed(_calculator.Calculate(s.Speed));
    }
}