using System;
using Sirenix.OdinInspector;

[Serializable]
public class SkillAmount
{
    [EnumToggleButtons]
    public AmountType Type;

    [ShowIf("Type", AmountType.Value)]
    [Indent]
    public int ValueAmount;

    [ShowIf("Type", AmountType.Percent)]
    [Indent]
    [SuffixLabel("%", overlay: true)] // 필드 옆에 % 표시를 붙여 단위를 명확히 합니다.
    public float PercentValue;

    [ShowIf("Type", AmountType.Fix)]
    [Indent]
    public int FixValue;

    public SkillAmountData ToData() => new(Type, ValueAmount, PercentValue, FixValue);
}