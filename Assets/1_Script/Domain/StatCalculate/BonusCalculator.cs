using System.Collections.Generic;


public class BonusCalculator
{
    SortedDictionary<int, int> bounsData = new SortedDictionary<int, int>();

    public BonusCalculator(SortedDictionary<int, int> bounsData)
    {
        this.bounsData = bounsData;
    }

    public int CalculateBonus(int value)
    {
        int bonus = 0;
        foreach (var data in bounsData) // 제일 높은 값까지 갱신
        {
            if (value >= data.Key) bonus = data.Value;
            else break;
        }
        return bonus;
    }
}
