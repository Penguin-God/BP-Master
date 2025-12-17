using System.Collections.Generic;
using System.Linq;

public readonly struct SkillUI_Data
{
    public readonly SkillType ActionType;
    public readonly SkillAmountData AmountData;
    
    public readonly SkillConditionData Condition;
    public readonly SkillTargetRule Rule;

    public readonly Side TargetSide => Rule.TargetSide;
    public readonly TargetRange Range => Rule.TargetRange;

    public SkillUI_Data(SkillData skillData)
    {
        ActionType = skillData.TraitType;
        AmountData = skillData.AmountData;
        Condition = skillData.ConditionData;
        Rule = skillData.TargetRule;
    }
}

public class SkillTextBuilder
{
    readonly SkillConditionTextBuilder ConditionTextBuilder = new SkillConditionTextBuilder();
    readonly SkillTextConverter skillTextConverter;

    public SkillTextBuilder(SkillTextConverter skillTextConverter)
    {
        this.skillTextConverter = skillTextConverter;
    }

    public string BuildSkillText(IEnumerable<SkillUI_Data> traitDatas) => string.Join(", ", traitDatas.Select(x => BuildSkillText(x)));

    public string BuildSkillText(SkillUI_Data traitData)
    {
        var conditoin = ConditionTextBuilder.BuildConditionText(traitData.Condition);
        var space = string.IsNullOrEmpty(conditoin) ? "" : " ";

        var target = BuildTargetRuleText(traitData.TargetSide, traitData.Range);
        var action = skillTextConverter.BuildActionText(traitData.ActionType, traitData.AmountData);

        // 조건이 있으면 "조건 + 공백"을 앞에 붙이고, 없으면 그대로
        return $"{conditoin}{space}{target} {action}";
    }

    string BuildTargetRuleText(Side side, TargetRange range) => range == TargetRange.All ? $"{SideText(side)} 전체" : $"{SideText(side)} {CountText(range)}의";
    string SideText(Side side) => side switch
    {
        Side.Self => "아군",
        Side.Opponent => "적군",
        Side.All => "양팀",
        _ => "대상 없음"
    };

    string CountText(TargetRange range) => range switch
    {
        TargetRange.Single => "하나",
        TargetRange.Double => "둘",
        TargetRange.Triple => "셋",
        _ => string.Empty
    };
}


public class SkillConditionTextBuilder
{
    public string BuildConditionText(SkillConditionData conditionData) => conditionData.ConditionType switch
    {
        ConditionType.None => "",
        ConditionType.Threshold => BuildThresholdText(conditionData.StatType, conditionData.Threshold),
        ConditionType.Compare => BuildCompareText(conditionData.StatType),
        ConditionType.Trait => BuildTriatText(conditionData.TraitType),
    };

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

    string BuildTriatText(TraitType traitType) => $"특성이 {new ChampionStatusTextBuilder().BuildTraitText(traitType)}인";
}

public readonly struct AmountChangeTextModel
{
    public readonly string Increased;
    public readonly string Decreased;
    public readonly string Fix;

    public AmountChangeTextModel(string increased, string decreased, string fix)
    {
        Increased = increased;
        Decreased = decreased;
        Fix = fix;
    }
}

public class SkillAmountTextBuilder
{
    readonly AmountChangeTextModel _changeText;

    public SkillAmountTextBuilder(AmountChangeTextModel changeText)
    {
        _changeText = changeText;
    }

    public string BuildAmountText(SkillAmountData data) => data.Type switch
    {
        AmountType.Value => data.ValueAmount.ToString(),
        AmountType.Percent => $"{ToPercentInt(data.PercentValue)}%",
        AmountType.Fix => data.FixValue.ToString(),
        _ => ""
    };

    public string BuildChangeText(SkillAmountData data) => data.Type switch
    {
        AmountType.Fix => _changeText.Fix,
        AmountType.Value => data.ValueAmount < 0 ? _changeText.Decreased : _changeText.Increased,
        AmountType.Percent => data.PercentValue < 0 ? _changeText.Decreased : _changeText.Increased,
        _ => ""
    };

    int ToPercentInt(float value) => (int)System.MathF.Round(value * 100f);
}
