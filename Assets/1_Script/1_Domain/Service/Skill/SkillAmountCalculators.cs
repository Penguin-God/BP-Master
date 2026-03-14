using System;

public class ValueCalculator : ISkillAmountCalculator
{
    readonly int Amount;
    public ValueCalculator(int amount) => this.Amount = amount;
    public int Calculate(int currentValue) => Amount;
}

public class PercentCalculator : ISkillAmountCalculator
{
    readonly float Percent;
    public PercentCalculator(float percent) => this.Percent = percent;

    public int Calculate(int currentValue)
    {
        float raw = currentValue * Percent;
        return (int)Math.Round(raw, MidpointRounding.AwayFromZero);
    }
}

public class FixCalculator : ISkillAmountCalculator
{
    readonly int FixValue;
    public FixCalculator(int amount) => this.FixValue = amount;

    public int Calculate(int currentValue) => FixValue - currentValue;
}
