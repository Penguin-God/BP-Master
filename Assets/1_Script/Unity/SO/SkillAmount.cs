using System;

public enum AmountType { None, Value, Percent, Fix }

[Serializable]
public class SkillAmount
{
    public AmountType Type;
    public int ValueAmount;
    public float PercentValue;
    public int FixValue;
}
