using System;

public readonly struct StatChangeTextData
{
    public readonly string Increased;
    public readonly string Decreased;
    public readonly string Fix;

    public StatChangeTextData(string increased, string decreased, string fix)
    {
        Increased = increased;
        Decreased = decreased;
        Fix = fix;
    }
}

public static class AmountTextBuilder
{
    public static string BuildAmountText(SkillAmountData data) => data.Type switch
    {
        AmountType.Value => MathF.Abs(data.ValueAmount).ToString(),
        AmountType.Percent => $"{ToPercentInt(MathF.Abs(data.PercentValue))}%",
        AmountType.Fix => data.FixValue.ToString(),
        _ => ""
    };

    static int ToPercentInt(float value) => (int)System.MathF.Round(value * 100f);

    public static string BuildChangeText(SkillAmountData data, StatChangeTextData changeText) => data.Type switch
    {
        AmountType.Fix => changeText.Fix,
        AmountType.Value => data.ValueAmount < 0 ? changeText.Decreased : changeText.Increased,
        AmountType.Percent => data.PercentValue < 0 ? changeText.Decreased : changeText.Increased,
        _ => ""
    };

    public static string BuildStatText(StatType statType) => statType switch
    {
        StatType.Attack => "공격력",
        StatType.Defense => "방어력",
        StatType.Speed => "속도",
        _ => ""
    };
}
