
public class ActionTextConverter : ISkillActionTextBuilder
{
    readonly string Template;
    readonly StatChangeTextData amountTextBuilder;
    readonly SkillConvertKey skillConvertKey;
    public ActionTextConverter(string template, StatChangeTextData skillStatChangeTextData, SkillConvertKey skillConvertKey)
    {
        Template = template;
        this.amountTextBuilder = skillStatChangeTextData;
        this.skillConvertKey = skillConvertKey;
    }

    public string BuildText(SkillType skillType, SkillAmountData data)
    {
        SkillConvertKey value = new SkillConvertKey("", "", AmountTextBuilder.BuildStatText(data.StatType), AmountTextBuilder.BuildAmountText(data), AmountTextBuilder.BuildChangeText(data, amountTextBuilder));
        return SkillTemplateHelper.ReplaceTemplate(Template, skillConvertKey.Values, value.Values);
    }
}
