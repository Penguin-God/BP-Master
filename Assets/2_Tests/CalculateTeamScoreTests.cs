using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CalculateTeamScoreTests
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
        if(rangeData == null) rangeData = new SortedDictionary<int, int>();
        if(speedData == null) speedData = new SortedDictionary<int, int>();
        return new TeamScoreCalculator(rangeData, speedData);
    }
}
