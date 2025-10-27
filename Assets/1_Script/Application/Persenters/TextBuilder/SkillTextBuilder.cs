using System;
using System.Collections.Generic;
using System.Linq;

public readonly struct TraitUI_Data
{
    public readonly SkillType TraitType;
    public readonly int Amount;

    public readonly SkillConditionData Condition;
    public readonly TraitTargetRule Rule;

    public readonly Side TargetSide => Rule.TargetSide;
    public readonly TargetRange Range => Rule.TargetRange;

    public TraitUI_Data(SkillType traitType, int amount, SkillConditionData conditionData, TraitTargetRule traitTargetRule)
    {
        TraitType = traitType;
        Amount = amount;
        Condition = conditionData;
        Rule = traitTargetRule;
    }
}

public class SkillTextBuilder
{
    public string BuildSkillText(IEnumerable<TraitUI_Data> traitDatas) => string.Join(", ", traitDatas.Select(x => BuildTraitText(x)));

    public string BuildTraitText(TraitUI_Data traitData)
    {
        var conditoin = BuildConditionText(traitData.Condition);
        var space = string.IsNullOrEmpty(conditoin) ? "" : " ";

        var target = BuildTargetRuleText(traitData.TargetSide, traitData.Range);
        var action = BuildActionText(traitData.TraitType, traitData.Amount);

        // 조건이 있으면 "조건 + 공백"을 앞에 붙이고, 없으면 그대로
        return $"{conditoin}{space}{target} {action}";
    }

    string BuildActionText(SkillType traitType, int amount) => (traitType) switch
    {
        SkillType.AttackChanger => $"공격력 {Math.Abs(amount)} {GetChangeLabel(amount)}",
        SkillType.DefenseChanger => $"방어력 {Math.Abs(amount)} {GetChangeLabel(amount)}",
        SkillType.SpeedChanger => $"속도 {Math.Abs(amount)} {GetChangeLabel(amount)}",
        SkillType.DefenseFixer => $"방어력 {Math.Abs(amount)}으로 고정",
        SkillType.TraitExcluder => $"스탯은 특성으로 인한 변화를 무시",
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

    string BuildConditionText(SkillConditionData conditionData)
    {
        if (conditionData.ConditionType == ConditionType.Threshold) return BuildThresholdText(conditionData.StatType, conditionData.Threshold);
        else return BuildCompareText(conditionData.StatType);
    }

    string BuildThresholdText(StatConditionType conditionType, int threshold) => conditionType switch
    {
        StatConditionType.None => "",
        StatConditionType.AttackAtLeast => $"공격력 {threshold} 이상인",
        StatConditionType.AttackBelow => $"공격력 {threshold} 이하인",
        StatConditionType.DefenseAtLeast => $"방어력 {threshold} 이상인",
        StatConditionType.DefenseBelow => $"방어력 {threshold} 이하인",
        StatConditionType.SpeedAtLeast => $"속도 {threshold} 이상인",
        StatConditionType.SpeedBelow => $"속도 {threshold} 이하인",
        _ => ""
    };

    string BuildCompareText(StatConditionType conditionType) => conditionType switch
    {
        StatConditionType.None => "",
        StatConditionType.AttackAtLeast => $"공격력이 자신보다 높은",
        StatConditionType.AttackBelow => $"공격력이 자신보다 낮은",
        StatConditionType.DefenseAtLeast => $"방어력이 자신보다 높은",
        StatConditionType.DefenseBelow => $"방어력이 자신보다 낮은",
        StatConditionType.SpeedAtLeast => $"속도가 자신보다 높은",
        StatConditionType.SpeedBelow => $"속도가 자신보다 낮은",
        _ => ""
    };

    string GetChangeLabel(int amount) => amount > 0 ? "증가" : "감소";
}