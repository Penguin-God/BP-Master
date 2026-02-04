using System;

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