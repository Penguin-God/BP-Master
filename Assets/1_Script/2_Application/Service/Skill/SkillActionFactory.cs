using System;

public class SkillActionFactory : ISkillActionFactory
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
            SkillType.StatAbsorber => new DefenseAbsorber(caster, amountCalculator, amountData.StatType),
            SkillType.PickBuffer => new PickChampStatChanger(phaseActionEventDispatcher, statChanger, team),
            SkillType.Resonance => new Resonance(caster, amountData.PercentValue),
            SkillType.AmplifyChanger => new AmplifyChanger(amountData.PercentValue),
            SkillType.Doppelganger => new Doppelganger(caster),
            SkillType.FinalStatChanger => new FinalStatChanger(caster, phaseEventDispatcher, amountCalculator),
            SkillType.TraitExcluder => new SkillExcluder(),
            _ => throw new NotImplementedException($"Action not implemented: {actionType}")
        };
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
}