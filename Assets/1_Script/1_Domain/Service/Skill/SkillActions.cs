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
        int changeAmount = Calculator.Calculate(GetStatAmount(target.Stat));
        target.ChangeStat(GetChangeStat(changeAmount, target.Stat));
    }

    int GetStatAmount(ChampionStatData stat) => StatType switch
    {
        StatType.Attack => stat.Attack,
        StatType.Defense => stat.Defense,
        StatType.Speed => stat.Speed,
        _ => 0
    };

    ChampionStatData GetChangeStat(int amount, ChampionStatData stat) => StatType switch
    {
        StatType.Attack => stat.ChangeAttack(stat.Attack + amount),
        StatType.Defense => stat.ChangeDefense(stat.Defense + amount),
        StatType.Speed => stat.ChangeSpeed(stat.Speed + amount),
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
    readonly ISkillAmountCalculator AmountCalculator; // 기존 로직용
    readonly StatChanger _userChanger;
    readonly StatChanger _targetChanger;

    // 기존 생성자 (하위 호환성 유지)
    public DefenseAbsorber(ChampionStatus user, ISkillAmountCalculator amountCalculator)
    {
        User = user;
        AmountCalculator = amountCalculator;
    }

    // 새로운 생성자: StatChanger를 직접 주입받음
    public DefenseAbsorber(ChampionStatus user, StatChanger userChanger, StatChanger targetChanger)
    {
        User = user;
        _userChanger = userChanger;
        _targetChanger = targetChanger;
    }

    public void Do(ChampionStatus target)
    {
        // 새로운 생성자로 생성된 경우 StatChanger를 사용
        if (_userChanger != null && _targetChanger != null)
        {
            _targetChanger.Do(target);
            _userChanger.Do(User);
            return;
        }

        // 기존 로직 (하위 호환)
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
    readonly StatChanger _statChanger;
    readonly int _amount; // 기존 로직용

    // 기존 생성자
    public PickChampBuffer(PhaseActionEventDispatcher eventDispatcher, int amount)
    {
        this.eventDispatcher = eventDispatcher;
        this._amount = amount;
    }

    // 새로운 생성자: StatChanger를 직접 주입받음
    public PickChampBuffer(PhaseActionEventDispatcher eventDispatcher, StatChanger statChanger)
    {
        this.eventDispatcher = eventDispatcher;
        this._statChanger = statChanger;
    }

    public void Do(ChampionStatus target)
    {
        if (_statChanger != null)
        {
            eventDispatcher.OnChampionPick += (champ) => _statChanger.Do(champ.Status);
            return;
        }

        eventDispatcher.OnChampionPick += (champ) => champ.Status.AddAttackWithRate(_amount);
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