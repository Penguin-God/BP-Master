using System;

public interface ISkillActionFactory
{
    ISkillAction CreateAction(SkillType actionType, SkillAmountData amountData, ChampionStatus caster);
}

public class SkillActionFactory : ISkillActionFactory
{
    readonly PhaseActionEventDispatcher phaseActionEventDispatcher;
    readonly PhaseEventDispatcher phaseEventDispatcher;
    readonly Team Team;
    public SkillActionFactory(PhaseActionEventDispatcher phaseActionEventDispatcher, PhaseEventDispatcher phaseEventDispatcher)
    {
        this.phaseActionEventDispatcher = phaseActionEventDispatcher;
        this.phaseEventDispatcher = phaseEventDispatcher;
    }

    public SkillActionFactory(PhaseActionEventDispatcher phaseActionEventDispatcher, PhaseEventDispatcher phaseEventDispatcher, Team team)
    {
        this.phaseActionEventDispatcher = phaseActionEventDispatcher;
        this.phaseEventDispatcher = phaseEventDispatcher;
        Team = team;
    }

    public ISkillAction CreateAction(SkillType actionType, SkillAmountData amountData, ChampionStatus caster)
    {
        var amountCalculator = SkillAmountCalculatorFactory.Create(amountData);
        var statChanger = new StatChanger(amountData.StatType, amountCalculator);

        return actionType switch
        {
            SkillType.StatChanger => statChanger,
            SkillType.StatAbsorber => new DefenseAbsorber(caster, amountCalculator, amountData.StatType),
            SkillType.PickBuffer => new PickChampStatChanger(phaseActionEventDispatcher, statChanger, Team),
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