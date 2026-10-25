
public class ActionTextConverter : ISkillActionTextBuilder
{
    readonly string Template;
    readonly SkillAmountTextBuilder amountTextBuilder;
    readonly SkillConvertKey skillConvertKey;
    public ActionTextConverter(string template, SkillAmountTextBuilder skillAmountTextBuilder, SkillConvertKey skillConvertKey)
    {
        Template = template;
        this.amountTextBuilder = skillAmountTextBuilder;
        this.skillConvertKey = skillConvertKey;
    }

    public string BuildText(SkillType skillType, SkillAmountData data)
    {
        SkillConvertKey value = new SkillConvertKey("", "", amountTextBuilder.BuildStatText(data.StatType), amountTextBuilder.BuildAmountText(data), amountTextBuilder.BuildChangeText(data));
        return SkillTemplateHelper.ReplaceTemplate(Template, skillConvertKey.Values, value.Values);
    }
}
