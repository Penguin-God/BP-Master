
using NUnit.Framework;
using System.Collections.Generic;

public class CalculateStatScoreTests
{
    [Test]
    [TestCase(0, 0, 0)]
    [TestCase(17, 25, 150)]
    [TestCase(12, 500, 100)]
    [TestCase(21, 21, 160)]
    public void 스탯_구간별_보너스(int range, int speed, int expected)
    {
        SortedDictionary<int, int> bonusData = new SortedDictionary<int, int>()
        {
            { 15, 50 },
            { 20, 80 },
            { 25, 100 }
        };
        StatScoreCalculator sut = new StatScoreCalculator(bonusData, bonusData);
        int result = sut.CalculateScore(0, 0, range, speed);

        Assert.AreEqual(expected, result);
    }
}
