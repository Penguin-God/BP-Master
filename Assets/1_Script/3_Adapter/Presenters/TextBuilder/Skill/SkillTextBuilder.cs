using Codice.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;

public interface ISkillActionTextBuilder
{
    string BuildText(SkillType skillType, SkillAmountData data);
}


public record SkillConvertKey(string Condition, string Rule, string Stat, string Value, string Action)
{
    public IReadOnlyList<string> Values => new[] { Condition, Rule, Stat, Value, Action };
}

public static class SkillTemplateHelper
{
    public static string ReplaceTemplate(string template, IReadOnlyList<string> keys, IReadOnlyList<string> values)
    {
        for (var i = 0; i < keys.Count; i++)
            template = template.Replace(keys[i], values[i]);
        return Regex.Replace(template, @"\s+", " ").Trim();
    }
}
public class SkillTextBuilder
{
    readonly ISkillActionTextBuilder actionTextBuilder;

    // 새 생성자에서 사용할 필드
    readonly string Template;
    readonly SkillConvertKey skillConvertKey;

    public SkillTextBuilder(ISkillActionTextBuilder actionTextBuilder) => this.actionTextBuilder = actionTextBuilder;

    public SkillTextBuilder(string template, SkillConvertKey skillConvertKey)
    {
        Template = template;
        this.skillConvertKey = skillConvertKey;
    }

    string _BuildSkillText(SkillViewModel skillData)
    {
        string conditoin = SkillConditionTextBuilder.BuildConditionText(skillData.ConditionData);
        string target = BuildTargetRuleText(skillData.SelectRule.TargetSide, skillData.SelectRule.TargetRange);
        string stat = AmountTextBuilder.BuildStatText(skillData.AmountData.StatType);
        string amount = AmountTextBuilder.BuildAmountText(skillData.AmountData);
        string change = AmountTextBuilder.BuildChangeText(skillData.AmountData, default);

        var replaceValue = new SkillConvertKey(conditoin, target, stat, amount, change);
        return SkillTemplateHelper.ReplaceTemplate(Template, skillConvertKey.Values, replaceValue.Values);
    }

    public string BuildSkillText(IEnumerable<SkillViewModel> skillDatas) => string.Join(", ", skillDatas.Select(x => BuildSkillText(x)));

    string BuildSkillText(SkillViewModel skillData)
    {
        var conditoin = SkillConditionTextBuilder.BuildConditionText(skillData.ConditionData);
        var space = string.IsNullOrEmpty(conditoin) ? "" : " ";

        var target = BuildTargetRuleText(skillData.SelectRule.TargetSide, skillData.SelectRule.TargetRange);
        var action = actionTextBuilder.BuildText(skillData.SkillType, skillData.AmountData);

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