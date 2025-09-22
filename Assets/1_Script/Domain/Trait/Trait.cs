using System;

public enum Side { Self, Opponent, All }
public enum TargetRange
{
    None,
    Single,
    All,
}

public readonly struct TraitTargetRule
{
    public readonly Side TargetSide;
    public readonly TargetRange TargetRange;

    public TraitTargetRule(Side targetSide, TargetRange targetRange)
    {
        TargetSide = targetSide;
        TargetRange = targetRange;
    }
}

public class TraitData
{
    public readonly ITraitAction Action;
    public readonly TraitConditionType ConditionType;
    public readonly int Threshold;

    public TraitData(ITraitAction action, TraitConditionType conditionType = TraitConditionType.None, int threshold = 0)
    {
        Action = action;
        ConditionType = conditionType;
        Threshold = threshold;
    }
}

public class TraitExecutor
{
    readonly TraitConditionChecker conditionChecker = new TraitConditionChecker();
    readonly TraitData traitData;

    public TraitExecutor() { }
    public TraitExecutor(TraitData traitData) => this.traitData = traitData;

    public void ExecuteTrait(Champion champion, TraitData traitData)
    {
        if (CanExecute(champion, traitData))
            traitData.Action.Do(champion);
    }

    // 새 오버로드: 내부에 들고 있는 구성 데이터로 실행
    public void ExecuteTrait(Champion champion)
    {
        ExecuteTrait(champion, traitData);
    }

    bool CanExecute(Champion champion, TraitData traitData) =>
        conditionChecker.CheckCondition(traitData.ConditionType, champion.StatData, traitData.Threshold);
}