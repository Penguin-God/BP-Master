using System;

public class TraitExecutorFactory
{
    public TraitExecutor CreateExecutor(TraitData traitData, ChampionStatData useChamp)
    {
        ITraitAction action = TraitActionFactory.CreateAction(traitData.TraitType, traitData.Amount);
        ITraitConditionChecker checker = TraitCondtionCheckerFactory.CreateChecker(traitData.ConditionData, useChamp);
        return new TraitExecutor(action, checker);
    }
}

public static class TraitActionFactory
{
    public static ITraitAction CreateAction(TraitType actionType, int amount)
    {
        return actionType switch
        {
            TraitType.AttackChanger => new AttackChanger(amount),
            TraitType.DefenseChanger => new DefenseChanger(amount),
            TraitType.SpeedChanger => new SpeedChanger(amount),
            TraitType.DefenseFixer => new DefenseFixer(amount),
            TraitType.TraitExcluder => new TraitExcluder(),
            _ => throw new NotImplementedException($"Action not implemented: {actionType}")
        };
    }
}

public static class TraitCondtionCheckerFactory
{
    public static ITraitConditionChecker CreateChecker(TraitConditionData data, ChampionStatData useChamp)
    {
        return data.CheckerType switch
        {
            ConditionCheckerType.None => new NullChecker(),
            ConditionCheckerType.Threshold => new StatThresholdChecker(data.ConditionType, data.Threshold),
            ConditionCheckerType.Compare => new StatComparisonChecker(data.ConditionType, useChamp),
            _ => throw new NotImplementedException($"Action not implemented: {data.CheckerType}")
        };
    }
}
