using System;

public static class TraitExecutorFactory
{
    public static TraitExecutor Create(
        TraitType actionType,
        int amount,
        TraitConditionType conditionType = TraitConditionType.None,
        int threshold = 0)
    {
        ITraitAction action = actionType switch
        {
            TraitType.AttackChanger => new AttackChanger(amount),
            TraitType.DefenseChanger => new DefenseChanger(amount),
            TraitType.SpeedChanger => new SpeedChanger(amount),
            _ => throw new NotImplementedException($"Action not implemented: {actionType}")
        };

        var traitData = new TraitData(action, conditionType, threshold);
        return new TraitExecutor(traitData);
    }
}
