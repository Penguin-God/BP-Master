using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CalculateScoreTests
{
    [Test]
    public void ����_����_������_���ʽ�()
    {
        Dictionary<int, int> bonusData = new Dictionary<int, int>()
        {
            { 15, 50 },
            { 20, 80 },
            { 25, 100 }
        };
        TeamScoreCalculator sut = new(bonusData);
        Champion[] team = new Champion[] { new (0, 0, 8, 15), new (0, 0, 9, 10) };
        int result = sut.CalculateScore(team);

        Assert.AreEqual(150, result);
    }

    [Test]
    public void ������_������_��()
    {
        TeamScoreCalculator sut = new(null);
        Champion[] team = new Champion[] { new (10, 30, 0, 0), new (20, 50, 0, 0) };
        
        int result = sut.CalculateScore(team);

        Assert.AreEqual(110, result);
    }
}
