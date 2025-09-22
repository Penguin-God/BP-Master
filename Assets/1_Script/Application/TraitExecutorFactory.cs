using System;

public static class TraitExecutorFactory
{
    public static TraitExecutor CreateExecutor(TraitData traitData)
    {
        ITraitAction action = CreateAction(traitData.TraitType, traitData.Amount);
        return new TraitExecutor(action, traitData.ConditionType, traitData.Threshold);
    }

    public static ITraitAction CreateAction(TraitType actionType, int amount)
    {
        return actionType switch
        {
            TraitType.AttackChanger => new AttackChanger(amount),
            TraitType.DefenseChanger => new DefenseChanger(amount),
            TraitType.SpeedChanger => new SpeedChanger(amount),
            _ => throw new NotImplementedException($"Action not implemented: {actionType}")
        };
    }
}
