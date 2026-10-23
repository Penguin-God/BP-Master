using System;

public class SkillActionFactory
{
    readonly BanPickEventDispatcher phaseActionEventDispatcher;
    readonly PhaseEventDispatcher phaseEventDispatcher;

    public SkillActionFactory(BanPickEventDispatcher phaseActionEventDispatcher, PhaseEventDispatcher phaseEventDispatcher)
    {
        this.phaseActionEventDispatcher = phaseActionEventDispatcher;
        this.phaseEventDispatcher = phaseEventDispatcher;
    }

    public ISkillTrigger CreateTrigger(SkillUseTimingType timingType, Team team)
        => SkillTriggerFactory.Create(timingType, phaseActionEventDispatcher, phaseEventDispatcher, team);

    public ISkillAction CreateAction(SkillData skillData, ChampionStatus caster, ChampionStatus target, Team team)
    {
        var statChanger = CreateStatChangerActor(skillData, caster, target);

        return skillData.SkillType switch
        {
            SkillType.StatChanger => statChanger,
            SkillType.FinalStatChanger => new FinalStatChanger(caster, phaseEventDispatcher, skillData.Calculator),
            SkillType.PickBuffer => new PickChampStatChanger(phaseActionEventDispatcher, statChanger, team),
            SkillType.OppentPickStatChanger => new OppentPickChampStatChanger(phaseActionEventDispatcher, statChanger, team),
            _ => throw new NotImplementedException($"생성에 가정되지 않는 스킬 : {skillData.SkillType}")
        };
    }

    StatChanger CreateStatChangerActor(SkillData skillData, ChampionStatus caster, ChampionStatus select)
        =>  new StatChanger(skillData.StatType, SkillFuncProvider.AmountProvider(skillData, select, caster), SkillFuncProvider.TargetProvider(skillData, select, caster));
}

public static class SkillFuncProvider
{
    public static Func<ChampionStatus> TargetProvider(SkillData skillData, ChampionStatus select, ChampionStatus caster)
        => () => skillData.SkillApplyType switch
        {
            SkillApplyType.Select => select,
            SkillApplyType.Triggered => select, // 이벤트로 전달받은 챔피언이 select로 들어옴
            _ => caster
        };

    public static Func<int> AmountProvider(SkillData skillData, ChampionStatus select, ChampionStatus caster)
    {
        int refValue = skillData.SkillValueType switch
        {
            SkillValueType.SelfRef => caster.GetStatAmount(skillData.StatType),
            SkillValueType.SelectRef => select.GetStatAmount(skillData.StatType),
            _ => TargetProvider(skillData, select, caster)().GetStatAmount(skillData.StatType)
        };

        return () => skillData.Calculator.Calculate(refValue);
    }
}


public static class SkillTriggerFactory
{
    public static ISkillTrigger Create(SkillUseTimingType timingType,BanPickEventDispatcher bpDispatcher,PhaseEventDispatcher phaseDispatcher,Team team) 
        => timingType switch
        {
            SkillUseTimingType.Immediate => new ImmediateTrigger(),
            SkillUseTimingType.OnSelfPick => new OnAllyPickChampionTrigger(bpDispatcher, team),
            SkillUseTimingType.OnOpponentPick => new OnOpponentPickChampionTrigger(bpDispatcher, team),
            SkillUseTimingType.OnPhaseDone => new OnPhaseDoneTrigger(phaseDispatcher),
            _ => new ImmediateTrigger()
        };
}

public static class CalculatorFactory
{
    public static ISkillAmountCalculator Create(SkillAmountData amountData)
        => amountData.Type switch
        {
            AmountType.Value => new ValueCalculator(amountData.ValueAmount),
            AmountType.Percent => new PercentCalculator(amountData.PercentValue),
            AmountType.Fix => new FixCalculator(amountData.FixValue),
            _ => null,
        };

    public static ISkillAmountCalculator CreateValue(int value) => new ValueCalculator(value);
    public static ISkillAmountCalculator CreateFix(int fixValue) => new FixCalculator(fixValue);
    public static ISkillAmountCalculator CreatePercent(float percent) => new PercentCalculator(percent);
}
