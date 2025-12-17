using System.Collections.Generic;

public class SkillTextConverter
{
    readonly IReadOnlyDictionary<SkillType, string> textBySkill;

    public SkillTextConverter(IReadOnlyDictionary<SkillType, string> textBySkill)
    {
        this.textBySkill = textBySkill;
    }

    public string BuildActionText(SkillType skillType, SkillAmountData data)
    {
        if (textBySkill.TryGetValue(skillType, out var template) == false) return "";

        string valueText = new SkillAmountTextBuilder().BuildAmountText(data);
        string changeText = BuildChangeText(data);

        return template
            .Replace("{Value}", valueText)
            .Replace("{Change}", changeText);
    }

    static string BuildChangeText(SkillAmountData data)
    {
        float signed = data.Type switch
        {
            AmountType.Value => data.ValueAmount,
            AmountType.Percent => data.PercentValue,
            _ => 0f
        };

        if (signed > 0f) return "증가";
        if (signed < 0f) return "감소";
        return "";
    }
}
