using System.Collections.Generic;

public class SkillTextConverter
{
    readonly IReadOnlyDictionary<SkillType, string> textBySkill;
    readonly SkillAmountTextBuilder skillAmountTextBuilder;
    public SkillTextConverter(IReadOnlyDictionary<SkillType, string> textBySkill, SkillAmountTextBuilder skillAmountTextBuilder)
    {
        this.textBySkill = textBySkill;
        this.skillAmountTextBuilder = skillAmountTextBuilder;
    }

    public string BuildActionText(SkillType skillType, SkillAmountData data)
    {
        if (textBySkill.TryGetValue(skillType, out var template) == false) return "";

        string valueText = skillAmountTextBuilder.BuildAmountText(data).Replace("-", string.Empty);
        string changeText = skillAmountTextBuilder.BuildChangeText(data);

        return template
            .Replace("{Value}", valueText)
            .Replace("{Change}", changeText);
    }
}
