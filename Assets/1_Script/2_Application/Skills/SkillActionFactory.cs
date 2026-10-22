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

    public ISkillAction CreateAction(SkillType actionType, SkillAmountData amountData, ChampionStatus caster, Team team)
    {
        var amountCalculator = SkillAmountCalculatorFactory.Create(amountData);
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
            _ => throw new NotImplementedException($"Action not implemented: {actionType}")
        };
    }

    public ISkillActor CreateActor(SkillData skillData, ChampionStatus caster, ChampionStatus target, Team team)
    {
        return skillData.SkillType switch
        {
            SkillType.StatChanger => CreateStatChangerActor(skillData, caster, target),
            _ => throw new NotImplementedException($"Actor not yet migrated: {skillData.SkillType}")
        };
    }

    StatChanger CreateStatChangerActor(SkillData skillData, ChampionStatus caster, ChampionStatus target)
    {
        var statType = skillData.AmountData.StatType;

        Func<int> amountProvider = () =>
        {
            int refValue = skillData.SkillValueType switch
            {
                SkillValueType.SelfRef => caster.GetStatAmount(statType),
                SkillValueType.SelectRef => target.GetStatAmount(statType),
                _ => 0
            };
            return SkillAmountCalculatorFactory.Create(skillData.AmountData, target.Stat).Calculate(refValue);
        };

        Func<ChampionStatus> targetProvider = () =>
            skillData.SkillApplyType == SkillApplyType.Select ? target : caster;

        return new StatChanger(statType, amountProvider, targetProvider);
    }
}

public static class SkillAmountCalculatorFactory
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
}


public static class SkillValueProvider
{
    public static int GetReferenceValue(SkillValueType valueType, StatType statType, ChampionStatus caster, ChampionStatus target) => valueType switch
    {
        SkillValueType.SelfRef => caster.GetStatAmount(statType),
        SkillValueType.SelectRef => target.GetStatAmount(statType),
        _ => 0
    };
}