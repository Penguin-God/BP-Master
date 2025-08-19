using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CalculateScoreTests
{
    [Test]
    public void 스탯_총합_구간별_보너스()
    {
        SortedDictionary<int, int> bonusData = new SortedDictionary<int, int>()
        {
            { 15, 50 },
            { 20, 80 },
            { 25, 100 }
        };
        TeamScoreCalculator sut = CreateScoreCalculator(bonusData, bonusData);
        Champion[] team = new Champion[] { new (0, 0, 8, 15), new (0, 0, 9, 10) };
        int result = sut.CalculateScore(team);

        Assert.AreEqual(150, result);
    }

    [Test]
    public void 점수는_공방의_합()
    {
        TeamScoreCalculator sut = CreateScoreCalculator(null, null);
        Champion[] team = new Champion[] { new (10, 30, 0, 0), new (20, 50, 0, 0) };
        
        int result = sut.CalculateScore(team);

        Assert.AreEqual(110, result);
    }

    TeamScoreCalculator CreateScoreCalculator(SortedDictionary<int, int> rangeData = null, SortedDictionary<int, int> speedData = null)
    {
        rangeData ??= new SortedDictionary<int, int>();
        speedData ??= new SortedDictionary<int, int>();
        var statCal = new StatScoreCalculator(new SortedDictionary<int, int>(), new SortedDictionary<int, int>(), rangeData, speedData);
        return new TeamScoreCalculator(statCal);
    }

    [Test]
    [TestCase(0, 0, 0, 0, 0)]
    [TestCase(300, 300, 17, 25, 850)]
    [TestCase(1000, 1000, 12, 500, 2300)]
    [TestCase(0, 0, 21, 21, 160)]
    public void 스탯_구간별_보너스(int attack, int defense, int range, int speed, int expected)
    {
        SortedDictionary<int, int> bonusData1 = new SortedDictionary<int, int>()
        {
            { 300, 50 },
            { 400, 80 },
            { 500, 100 }
        };

        SortedDictionary<int, int> bonusData = new SortedDictionary<int, int>()
        {
            { 15, 50 },
            { 20, 80 },
            { 25, 100 }
        };
        StatScoreCalculator sut = new StatScoreCalculator(bonusData1, bonusData1, bonusData, bonusData);
        int result = sut.CalculateScore(attack, defense, range, speed);

        Assert.AreEqual(expected, result);
    }

    [Test]
    public void 점수는_공방과_보너스의_합()
    {
        var bonusData = new SortedDictionary<int, int>();
        bonusData.Add(15, 50);
        StatScoreCalculator sut = new(bonusData, bonusData, bonusData, bonusData);
        
        int result = sut.CalculateScore(80, 30, 15, 15);

        Assert.AreEqual(310, result);
    }
}
