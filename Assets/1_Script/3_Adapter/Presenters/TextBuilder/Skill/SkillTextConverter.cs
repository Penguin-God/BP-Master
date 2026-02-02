using System.Collections.Generic;

public record SkillConvertKeyRecord(string Value, string Action, string Stat);

public class SkillTextConverter : ISkillActionTextBuilder
{
    readonly IReadOnlyDictionary<SkillType, string> textBySkill;
    readonly SkillAmountTextBuilder skillAmountTextBuilder;
    readonly SkillConvertKeyRecord skillConvertKey;
    public SkillTextConverter(IReadOnlyDictionary<SkillType, string> textBySkill, SkillAmountTextBuilder skillAmountTextBuilder, SkillConvertKeyRecord skillConvertKey)
    {
        this.textBySkill = textBySkill;
        this.skillAmountTextBuilder = skillAmountTextBuilder;
        this.skillConvertKey = skillConvertKey;
    }

    public string BuildText(SkillType skillType, SkillAmountData data)
    {
        if (textBySkill.TryGetValue(skillType, out var template) == false) return "";

        string valueText = skillAmountTextBuilder.BuildAmountText(data).Replace("-", string.Empty);
        string actionText = skillAmountTextBuilder.BuildChangeText(data);

        return template
            .Replace(skillConvertKey.Value, valueText)
            .Replace(skillConvertKey.Action, actionText)
            .Replace(skillConvertKey.Stat, skillAmountTextBuilder.BuildStatText(data.StatType));
    }
}
