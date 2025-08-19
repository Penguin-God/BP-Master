
using NUnit.Framework;
using System.Collections.Generic;

public class CalculateStatScoreTests
{
    [Test]
    public void ����_������_���ʽ�()
    {
        SortedDictionary<int, int> bonusData = new SortedDictionary<int, int>()
        {
            { 15, 50 },
            { 20, 80 },
            { 25, 100 }
        };
        StatScoreCalculator sut = new StatScoreCalculator(bonusData, bonusData);
        int result = sut.CalculateScore(0, 0, 17, 25);

        Assert.AreEqual(150, result);
    }
}
