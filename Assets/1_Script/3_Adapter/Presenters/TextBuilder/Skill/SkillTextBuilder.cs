using System.Collections.Generic;
using System.Linq;

public interface ISkillActionTextBuilder
{
    string BuildText(SkillType skillType, SkillAmountData data);
}

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
        ActionType = skillData.SkillType;
        AmountData = skillData.AmountData;
        Condition = skillData.ConditionData;
        Rule = skillData.TargetRule;
    }
}

public class SkillTextBuilder
{
    readonly SkillConditionTextBuilder ConditionTextBuilder = new SkillConditionTextBuilder();
    readonly ISkillActionTextBuilder actionTextBuilder;

    public SkillTextBuilder(ISkillActionTextBuilder actionTextBuilder) => this.actionTextBuilder = actionTextBuilder;

    public string BuildSkillText(IEnumerable<SkillUI_Data> traitDatas) => string.Join(", ", traitDatas.Select(x => BuildSkillText(x)));

    public string BuildSkillText(SkillUI_Data skillData)
    {
        var conditoin = ConditionTextBuilder.BuildConditionText(skillData.Condition);
        var space = string.IsNullOrEmpty(conditoin) ? "" : " ";

        var target = BuildTargetRuleText(skillData.TargetSide, skillData.Range);
        var action = actionTextBuilder.BuildText(skillData.ActionType, skillData.AmountData);

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