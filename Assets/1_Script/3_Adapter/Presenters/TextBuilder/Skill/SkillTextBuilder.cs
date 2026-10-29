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
        string target = sideTextData.BuildSelectRuleText(skillData.SelectRule.TargetSide, skillData.SelectRule.TargetRange);
        string stat = AmountTextBuilder.BuildStatText(skillData.AmountData.StatType, statTextData);
        string amount = AmountTextBuilder.BuildAmountText(skillData.AmountData);
        string change = AmountTextBuilder.BuildChangeText(skillData.AmountData, statChangeData);

        var replaceValue = new SkillConvertKey(conditoin, target, stat, amount, change);

        string templateToUse = string.IsNullOrEmpty(skillData.CustomTemplate) ? Template : skillData.CustomTemplate;
        return SkillTemplateHelper.ReplaceTemplate(templateToUse, skillConvertKey.Values, replaceValue.Values);
    }

    // 조건과 타깃 포함 여부를 외부에서 결정할 수 있는 신규 헬퍼 메서드
    string BuildSkillText(SkillViewModel skillData, bool includeConditionAndTarget)
    {
        string condition = includeConditionAndTarget ? SkillConditionTextBuilder.BuildConditionText(skillData.ConditionData) : "";
        string target = includeConditionAndTarget ? sideTextData.BuildSelectRuleText(skillData.SelectRule.TargetSide, skillData.SelectRule.TargetRange) : "";
        string stat = AmountTextBuilder.BuildStatText(skillData.AmountData.StatType, statTextData);
        string amount = AmountTextBuilder.BuildAmountText(skillData.AmountData);
        string change = AmountTextBuilder.BuildChangeText(skillData.AmountData, statChangeData);

        var replaceValue = new SkillConvertKey(condition, target, stat, amount, change);

        string templateToUse = string.IsNullOrEmpty(skillData.CustomTemplate) ? Template : skillData.CustomTemplate;
        return SkillTemplateHelper.ReplaceTemplate(templateToUse, skillConvertKey.Values, replaceValue.Values);
    }

    // 기존 동작 보존
    public string BuildSkillText(IEnumerable<SkillViewModel> skillDatas) => string.Join(", ", skillDatas.Select(x => BuildSkillText(x)));

    // 신규 메서드: 동일한 조건과 타깃 룰을 가진 스킬들을 묶어서 중복 문구를 제거한 텍스트 생성
    public string BuildGroupedSkillText(IEnumerable<SkillViewModel> skillDatas)
    {
        if (skillDatas == null) return string.Empty;

        var results = new List<string>();
        var groups = skillDatas.GroupBy(x => (x.ConditionData, x.SelectRule));

        foreach (var group in groups)
        {
            var isFirst = true;
            foreach (var skill in group)
            {
                results.Add(BuildSkillText(skill, includeConditionAndTarget: isFirst));
                isFirst = false;
            }
        }

        return string.Join(", ", results);
    }
}