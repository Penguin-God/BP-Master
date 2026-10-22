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
    {
        var statType = skillData.StatType;
        ChampionStatus TargetProvider() => skillData.SkillApplyType == SkillApplyType.Select ? select : caster;

        int AmountProvider()
        {
            int refValue = skillData.Calculator is FixCalculator ? TargetProvider().GetStatAmount(statType) 
                : skillData.SkillValueType switch
                {
                    SkillValueType.SelfRef => caster.GetStatAmount(statType),
                    SkillValueType.SelectRef => select.GetStatAmount(statType),
                    _ => TargetProvider().GetStatAmount(statType)
                };
            return skillData.Calculator.Calculate(refValue);
        };

        return new StatChanger(statType, AmountProvider, TargetProvider);
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

    public static ISkillAmountCalculator CreateValue(int value) => new ValueCalculator(value);
    public static ISkillAmountCalculator CreateFix(int fixValue) => new FixCalculator(fixValue);
    public static ISkillAmountCalculator CreatePercent(float percent) => new PercentCalculator(percent);
}
