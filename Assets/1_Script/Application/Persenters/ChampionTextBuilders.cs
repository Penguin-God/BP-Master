using System;
using System.Collections.Generic;
using System.Linq;

public readonly struct StatViewModel
{
    public readonly string Attack;
    public readonly string Defense;
    public readonly string Speed;
    
    public StatViewModel(string attack, string defense, string speed)
    {
        Attack = attack;
        Defense = defense;
        Speed = speed;
    }
}

public readonly struct TraitUI_Data
{
    public readonly TraitType TraitType;
    public readonly int Amount;

    public readonly TraitConditionData Condition;
    public readonly TraitTargetRule Rule;

    public readonly Side TargetSide => Rule.TargetSide;
    public readonly TargetRange Range => Rule.TargetRange;

    public TraitUI_Data(TraitType traitType, int amount, TraitConditionData conditionData, TraitTargetRule traitTargetRule)
    {
        TraitType = traitType;
        Amount = amount;
        Condition = conditionData;
        Rule = traitTargetRule;
    }
}

public class TraitTextBuilder
{
    public string BuildTraitText(IEnumerable<TraitUI_Data> traitDatas) => string.Join(", ", traitDatas.Select(x => BuildTraitText(x)));

    public string BuildTraitText(TraitUI_Data traitData)
    {
        var conditoin = BuildConditionText(traitData.Condition);
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
        TraitType.DefenseFixer => $"방어력 {Math.Abs(amount)}으로 고정",
        TraitType.TraitExcluder => $"스탯은 특성으로 인한 변화를 무시",
        _ => ""
    };

    string BuildTargetRuleText(Side side, TargetRange range) => (side, range) switch
    {
        (Side.Self, TargetRange.Single) => "선택한 아군 하나의",
        (Side.Self, TargetRange.Double) => "선택한 아군 둘의",
        (Side.Self, TargetRange.Triple) => "선택한 아군 셋의",
        (Side.Self, TargetRange.All) => "아군 전체",
        (Side.Opponent, TargetRange.Single) => "선택한 적군 하나의",
        (Side.Opponent, TargetRange.Double) => "선택한 적군 둘의",
        (Side.Opponent, TargetRange.Triple) => "선택한 적군 셋의",
        (Side.Opponent, TargetRange.All) => "적군 전체",
        (Side.All, TargetRange.All) => "양팀 전체",
        (Side.All, TargetRange.Single) => "선택한 하나의",
        (Side.All, TargetRange.Double) => "선택한 둘의",
        (Side.All, TargetRange.Triple) => "선택한 셋의",
        _ => "대상 없음"
    };

    string BuildConditionText(TraitConditionData conditionData)
    {
        if (conditionData.CheckerType == ConditionCheckerType.Threshold) return BuildThresholdText(conditionData.ConditionType, conditionData.Threshold);
        else return BuildCompareText(conditionData.ConditionType);
    }

    string BuildThresholdText(TraitConditionType conditionType, int threshold) => conditionType switch
    {
        TraitConditionType.None => "",
        TraitConditionType.AttackAtLeast => $"공격력 {threshold} 이상인",
        TraitConditionType.AttackBelow => $"공격력 {threshold} 이하인",
        TraitConditionType.DefenseAtLeast => $"방어력 {threshold} 이상인",
        TraitConditionType.DefenseBelow => $"방어력 {threshold} 이하인",
        TraitConditionType.SpeedAtLeast => $"속도 {threshold} 이상인",
        TraitConditionType.SpeedBelow => $"속도 {threshold} 이하인",
        _ => ""
    };

    string BuildCompareText(TraitConditionType conditionType) => conditionType switch
    {
        TraitConditionType.None => "",
        TraitConditionType.AttackAtLeast => $"공격력이 자신보다 높은",
        TraitConditionType.AttackBelow => $"공격력이 자신보다 낮은",
        TraitConditionType.DefenseAtLeast => $"방어력이 자신보다 높은",
        TraitConditionType.DefenseBelow => $"방어력이 자신보다 낮은",
        TraitConditionType.SpeedAtLeast => $"속도가 자신보다 높은",
        TraitConditionType.SpeedBelow => $"속도가 자신보다 낮은",
        _ => ""
    };

    string GetChangeLabel(int amount) => amount > 0 ? "증가" : "감소";
}

public class StatTextBuilder
{
    public StatViewModel CreateStatViewModel(ChampionStatData stat) => 
        new StatViewModel(
        $"공 {stat.Attack}",
        $"방 {stat.Defense}",
        $"속도 {stat.Speed}"
    );
}
