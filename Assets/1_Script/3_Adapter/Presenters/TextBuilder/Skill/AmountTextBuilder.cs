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

public readonly struct StatTextData
{
    public readonly string Attack;
    public readonly string Defense;
    public readonly string Speed;

    public StatTextData(string attack, string defense, string speed)
    {
        Attack = attack;
        Defense = defense;
        Speed = speed;
    }
}

public record SideTextData(string Self, string Opponent, string All, string None, string AllCardsText, string SelectCardsPrefix, string SelectCardsSuffix)
{
    public string BuildTargetRuleText(Side side, TargetRange range) => range switch
    {
        TargetRange.All => $"{SideText(side)}{AllCardsText}",
        _ => $"{SideText(side)}{SelectCardsPrefix}{CountText(range)}{SelectCardsSuffix}"
    };

    public string SideText(Side side) => side switch
    {
        Side.Self => Self,
        Side.Opponent => Opponent,
        Side.All => All,
        _ => None
    };

    static string CountText(TargetRange range) => range switch
    {
        TargetRange.Single => "하나",
        TargetRange.Double => "둘",
        TargetRange.Triple => "셋",
        _ => string.Empty
    };
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

    static int ToPercentInt(float value) => (int)MathF.Round(value * 100f);

    public static string BuildChangeText(SkillAmountData data, StatChangeTextData changeText) => data.Type switch
    {
        AmountType.Fix => changeText.Fix,
        AmountType.Value => data.ValueAmount < 0 ? changeText.Decreased : changeText.Increased,
        AmountType.Percent => data.PercentValue < 0 ? changeText.Decreased : changeText.Increased,
        _ => ""
    };

    public static string BuildStatText(StatType statType, StatTextData statText) => statType switch
    {
        StatType.Attack => statText.Attack,
        StatType.Defense => statText.Defense,
        StatType.Speed => statText.Speed,
        _ => ""
    };
}