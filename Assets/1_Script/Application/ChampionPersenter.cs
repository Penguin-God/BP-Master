using System;

public readonly struct ChampionViewModel
{
    public readonly string Attack;
    public readonly string Defense;
    public readonly string Speed;
    public readonly string Trait;

    public ChampionViewModel(string attack, string defense, string speed, string trait)
    {
        Attack = attack;
        Defense = defense;
        Speed = speed;
        Trait = trait;
    }
}

public struct TraitUI_Data
{
    public readonly TraitType TraitType;
    public readonly Side TargetSide;
    public readonly TargetRange Range;
    public readonly int Amount;
    public readonly TraitConditionType ConditionType;
    public readonly int Threshold;

    public TraitUI_Data(TraitType traitType, Side targetSide, TargetRange range, int amount, TraitConditionType conditionType, int threshold)
    {
        TraitType = traitType;
        TargetSide = targetSide;
        Range = range;
        Amount = amount;
        ConditionType = conditionType;
        Threshold = threshold;
    }
}

public class TraitPersenter
{
    public string BuildTraitText(TraitUI_Data traitData)
    {
        var conditoin = BuildTraitConditionText(traitData.ConditionType, traitData.Threshold);
        var space = string.IsNullOrEmpty(conditoin) ? "" : " ";

        var target = BuildTargetRuleText(traitData.TargetSide, traitData.Range);
        var action = BuildActionText(traitData.TraitType, traitData.Amount);

        // 조건이 있으면 "조건 + 공백"을 앞에 붙이고, 없으면 그대로
        return $"{conditoin}{space}{target} {action}";
    }

    string BuildActionText(TraitType traitType, int amount) => (traitType) switch
    {
        TraitType.AttackChanger => $"공격력 {Math.Abs(amount)} {GetChangeLabel(amount)}",
        TraitType.DefenseChanger => $"방어력 {Math.Abs(amount)} {GetChangeLabel(amount)}",
        TraitType.SpeedChanger => $"속도 {Math.Abs(amount)} {GetChangeLabel(amount)}",
        _ => ""
    };

    string BuildTargetRuleText(Side side, TargetRange range) => (side, range) switch
    {
        (Side.Self, TargetRange.Single) => "아군 단일 대상",
        (Side.Self, TargetRange.All) => "아군 전체",
        (Side.Opponent, TargetRange.Single) => "적군 단일 대상",
        (Side.Opponent, TargetRange.All) => "적군 전체",
        (Side.All, TargetRange.All) => "양팀 전체",
        _ => "대상 없음"
    };


    string BuildTraitConditionText(TraitConditionType conditionType, int threshold) => conditionType switch
    {
        TraitConditionType.None => "",
        TraitConditionType.AttackAtLeast => $"공격력이 {threshold} 이상인",
        TraitConditionType.AttackBelow => $"공격력이 {threshold} 이하인",
        TraitConditionType.DefenseAtLeast => $"방어력이 {threshold} 이상인",
        TraitConditionType.DefenseBelow => $"방어력이 {threshold} 이하인",
        TraitConditionType.SpeedAtLeast => $"속도 {threshold} 이상인",
        TraitConditionType.SpeedBelow => $"속도 {threshold} 이하인",
        _ => ""
    };

    string GetChangeLabel(int amount) => amount > 0 ? "증가" : "감소";
}

public class ChampionPersenter : TraitPersenter
{
    public ChampionViewModel CreateViewModel(ChampionStatData stat, TraitUI_Data traitData) => 
        new ChampionViewModel(
        $"공격력 : {stat.Attack}",
        $"방어력 : {stat.Defense}",
        $"속도 : {stat.Speed}",
        BuildTraitText(traitData)
    );
}
