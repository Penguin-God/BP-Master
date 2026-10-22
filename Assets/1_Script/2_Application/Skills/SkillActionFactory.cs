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

    public ISkillAction CreateAction(SkillData skillData, ChampionStatus caster, ChampionStatus target, Team team)
    {
        var amountData = skillData.AmountData;
        var amountCalculator = CalculatorFactory.Create(amountData);
        // var statChanger = new StatChanger(amountData.StatType, amountCalculator);
        var statChanger = CreateStatChangerActor(skillData, caster, target);

        return skillData.SkillType switch
        {
            SkillType.StatChanger => statChanger,
            SkillType.StatAbsorber => new StatAbsorber(caster, amountCalculator, amountData.StatType),
            SkillType.Resonance => new Resonance(caster, amountData.PercentValue),
            SkillType.Doppelganger => new Doppelganger(caster),
            SkillType.FinalStatChanger => new FinalStatChanger(caster, phaseEventDispatcher, amountCalculator),
            SkillType.PickBuffer => new PickChampStatChanger(phaseActionEventDispatcher, statChanger, team),
            SkillType.OppentPickStatChanger => new OppentPickChampStatChanger(phaseActionEventDispatcher, statChanger, team),
            _ => throw new NotImplementedException($"생성에 가정되지 않는 스킬 : {skillData.SkillType}")
        };
    }

    public ISkillAction CreateAction(SkillType actionType, SkillAmountData amountData, ChampionStatus caster, Team team)
    {
        var amountCalculator = CalculatorFactory.Create(amountData);
        var statChanger = new StatChanger(amountData.StatType, amountCalculator);

        return actionType switch
        {
            SkillType.StatChanger => statChanger,
            SkillType.StatAbsorber => new StatAbsorber(caster, amountCalculator, amountData.StatType),
            SkillType.Resonance => new Resonance(caster, amountData.PercentValue),
            SkillType.Doppelganger => new Doppelganger(caster),
            SkillType.FinalStatChanger => new FinalStatChanger(caster, phaseEventDispatcher, amountCalculator),
            SkillType.PickBuffer => new PickChampStatChanger(phaseActionEventDispatcher, statChanger, team),
            SkillType.OppentPickStatChanger => new OppentPickChampStatChanger(phaseActionEventDispatcher, statChanger, team),
            _ => throw new NotImplementedException($"생성에 가정되지 않는 스킬 : {actionType}")
        };
    }

    StatChanger CreateStatChangerActor(SkillData skillData, ChampionStatus caster, ChampionStatus target)
    {
        var statType = skillData.AmountData.StatType;

        int AmountProvider()
        {
            int refValue = skillData.SkillValueType switch
            {
                SkillValueType.SelfRef => caster.GetStatAmount(statType),
                SkillValueType.SelectRef => target.GetStatAmount(statType),
                _ => 0
            };
            return CalculatorFactory.Create(skillData.AmountData, target.Stat).Calculate(refValue);
        };

        Func<ChampionStatus> targetProvider = () => skillData.SkillApplyType == SkillApplyType.Select ? target : caster;

        return new StatChanger(statType, AmountProvider, targetProvider);
    }
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

    public static ISkillAmountCalculator Create(SkillAmountData amountData, ChampionStatData referenceStat)
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