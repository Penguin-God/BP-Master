using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
    readonly string Template;
    readonly SkillConvertKey skillConvertKey;
    readonly StatChangeTextData statChangeData;
    readonly StatTextData statTextData;
    readonly SideTextData sideTextData;

    public SkillTextBuilder(string template, StatChangeTextData statChangeData, StatTextData statTextData, SideTextData sideTextData, SkillConvertKey skillConvertKey)
    {
        Template = template;
        this.statChangeData = statChangeData;
        this.statTextData = statTextData;
        this.sideTextData = sideTextData;
        this.skillConvertKey = skillConvertKey;
    }

    string BuildSkillText(SkillViewModel skillData)
    {
        string conditoin = SkillConditionTextBuilder.BuildConditionText(skillData.ConditionData);
        string target = BuildTargetRuleText(skillData.SelectRule.TargetSide, skillData.SelectRule.TargetRange);
        string stat = AmountTextBuilder.BuildStatText(skillData.AmountData.StatType, statTextData);
        string amount = AmountTextBuilder.BuildAmountText(skillData.AmountData);
        string change = AmountTextBuilder.BuildChangeText(skillData.AmountData, statChangeData);

        var replaceValue = new SkillConvertKey(conditoin, target, stat, amount, change);

        string templateToUse = string.IsNullOrEmpty(skillData.CustomTemplate) ? Template : skillData.CustomTemplate;
        return SkillTemplateHelper.ReplaceTemplate(templateToUse, skillConvertKey.Values, replaceValue.Values);
    }

    public string BuildSkillText(IEnumerable<SkillViewModel> skillDatas) => string.Join(", ", skillDatas.Select(x => BuildSkillText(x)));

    string BuildTargetRuleText(Side side, TargetRange range) => range switch
    {
        TargetRange.All => $"{SideText(side)}의 모든 카드",
        _ => $"{SideText(side)}에서 선택한 카드 {CountText(range)}의"
    };

    string SideText(Side side) => side switch
    {
        Side.Self => sideTextData.Self,
        Side.Opponent => sideTextData.Opponent,
        Side.All => sideTextData.All,
        _ => sideTextData.None
    };

    string CountText(TargetRange range) => range switch
    {
        TargetRange.Single => "하나",
        TargetRange.Double => "둘",
        TargetRange.Triple => "셋",
        _ => string.Empty
    };
}