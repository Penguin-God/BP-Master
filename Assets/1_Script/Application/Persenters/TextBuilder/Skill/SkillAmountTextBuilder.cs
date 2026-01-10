public readonly struct AmountChangeTextModel
{
    public readonly string Increased;
    public readonly string Decreased;
    public readonly string Fix;

    public AmountChangeTextModel(string increased, string decreased, string fix)
    {
        Increased = increased;
        Decreased = decreased;
        Fix = fix;
    }
}

public class SkillAmountTextBuilder
{
    readonly AmountChangeTextModel changeText;

    public SkillAmountTextBuilder(AmountChangeTextModel changeText)
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

    int ToPercentInt(float value) => (int)System.MathF.Round(value * 100f);
}
