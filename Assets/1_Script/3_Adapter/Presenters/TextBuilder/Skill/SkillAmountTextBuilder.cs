public readonly struct AmountTextData
{
    public readonly string Increased;
    public readonly string Decreased;
    public readonly string Fix;

    public AmountTextData(string increased, string decreased, string fix)
    {
        Increased = increased;
        Decreased = decreased;
        Fix = fix;
    }
}

public class SkillAmountTextBuilder
{
    readonly AmountTextData changeText;

    public SkillAmountTextBuilder(AmountTextData changeText)
    {
        this.changeText = changeText;
    }

    public string BuildAmountText(SkillAmountData data) => data.Type switch
    {
        AmountType.Value => data.ValueAmount.ToString(),
        AmountType.Percent => $"{ToPercentInt(data.PercentValue)}%",
        AmountType.Fix => data.FixValue.ToString(),
        _ => ""
    };

    public string BuildChangeText(SkillAmountData data) => data.Type switch
    {
        AmountType.Fix => changeText.Fix,
        AmountType.Value => data.ValueAmount < 0 ? changeText.Decreased : changeText.Increased,
        AmountType.Percent => data.PercentValue < 0 ? changeText.Decreased : changeText.Increased,
        _ => ""
    };

    public string BuildStatText(StatType statType) => statType switch
    {
        StatType.Attack => "공격력",
        StatType.Defense => "방어력",
        StatType.Speed => "속도",
        _ => ""
    };

    int ToPercentInt(float value) => (int)System.MathF.Round(value * 100f);
}
