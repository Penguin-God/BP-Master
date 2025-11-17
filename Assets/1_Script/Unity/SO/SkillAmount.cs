using System;

[Serializable]
public class SkillAmount
{
    public AmountType Type;
    public int ValueAmount;
    public float PercentValue;
    public int FixValue;

    public SkillAmountData ToData() => new SkillAmountData(Type, ValueAmount, PercentValue, FixValue);
}
