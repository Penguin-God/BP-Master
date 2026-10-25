using System.Collections.Generic;

public record SkillConvertKeyRecord(string Value, string Action, string Stat);

public class ActionTextConverter : ISkillActionTextBuilder
{
    readonly IReadOnlyDictionary<SkillType, string> textBySkill;
    readonly SkillAmountTextBuilder amountTextBuilder;
    readonly SkillConvertKey skillConvertKey;
    public ActionTextConverter(IReadOnlyDictionary<SkillType, string> textBySkill, SkillAmountTextBuilder skillAmountTextBuilder, SkillConvertKey skillConvertKey)
    {
        this.textBySkill = textBySkill;
        this.amountTextBuilder = skillAmountTextBuilder;
        this.skillConvertKey = skillConvertKey;
    }

    public string BuildText(SkillType skillType, SkillAmountData data)
    {
        if (textBySkill.TryGetValue(skillType, out var template) == false) return "";

        SkillConvertKey value = new SkillConvertKey("", "", amountTextBuilder.BuildStatText(data.StatType), amountTextBuilder.BuildAmountText(data), amountTextBuilder.BuildChangeText(data));
        return SkillTemplateHelper.ReplaceTemplate(template, skillConvertKey.Values, value.Values);
    }
}
