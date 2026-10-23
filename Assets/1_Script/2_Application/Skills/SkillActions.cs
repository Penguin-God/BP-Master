using System;

public class StatChanger : ISkillAction
{
    readonly StatType statType;
    readonly Func<int> _amountProvider;
    readonly Func<ChampionStatus> _targetProvider;
    public StatChanger(StatType statType, Func<int> amountProvider, Func<ChampionStatus> targetProvider)
    {
        this.statType = statType;
        _amountProvider = amountProvider;
        _targetProvider = targetProvider;
    }

    public void Do(ChampionStatus target) => _targetProvider().AddStat(_amountProvider(), statType);
}

public interface ISkillTrigger
{
    void Register(Action execute);
}

public class ImmediateTrigger : ISkillTrigger
{
    public void Register(Action execute) => execute();
}

public class OnPickChampionTrigger : ISkillTrigger
{
    readonly BanPickEventDispatcher eventDispatcher;
    readonly Func<Team, bool> teamCondition;

    public OnPickChampionTrigger(BanPickEventDispatcher eventDispatcher, Func<Team, bool> teamCondition)
    {
        this.eventDispatcher = eventDispatcher;
        this.teamCondition = teamCondition;
    }

    public void Register(Action execute)
    {
        eventDispatcher.OnTeamChampionPick += (champion, team) =>
        {
            if (teamCondition(team))
                execute();
        };
    }
}

public class OnPhaseDoneTrigger : ISkillTrigger
{
    readonly PhaseEventDispatcher dispatcher;
    public OnPhaseDoneTrigger(PhaseEventDispatcher dispatcher) => this.dispatcher = dispatcher;
    public void Register(Action execute) => dispatcher.OnPhaseDone += execute;
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