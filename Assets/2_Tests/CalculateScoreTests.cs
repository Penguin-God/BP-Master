using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CalculateScoreTests
{
    [Test]
    public void ������_�����_���ʽ���_��()
    {
        SortedDictionary<int, int> champBonusData = new SortedDictionary<int, int>();
        champBonusData.Add(100, 20);

        SortedDictionary<int, int> teamBonusData1 = new SortedDictionary<int, int>();
        teamBonusData1.Add(300, 50);

        SortedDictionary<int, int> teamBonusData2 = new SortedDictionary<int, int>();
        teamBonusData2.Add(20, 30);

        var champCal = new ChampionBonusCalculator(champBonusData, champBonusData);
        var teamCal = new TeamBonusCalculator(teamBonusData1, teamBonusData1, teamBonusData2, teamBonusData2);
        TeamScoreCalculator sut = new TeamScoreCalculator(champCal, teamCal);
        Champion[] team = new Champion[] { new(150, 150, 10, 10), new(80, 200, 10, 15) };

        int result = sut.CalculateScore(team);

        // 580 + 60 + 110
        Assert.AreEqual(750, result);
    }

    [Test]
    public void ��_���ʽ���_����_����_���ʽ�_����()
    {
        SortedDictionary<int, int> bonusData1 = new SortedDictionary<int, int>()
        {
            { 300, 50 },
            { 400, 80 },
            { 500, 100 }
        };

        SortedDictionary<int, int> bonusData2 = new SortedDictionary<int, int>()
        {
            { 15, 50 },
            { 20, 80 },
            { 25, 100 }
        };

        Champion[] champions = new Champion[] { new(150, 150, 10, 10), new(200, 300, 10, 15) };
        TeamBonusCalculator sut = new TeamBonusCalculator(bonusData1, bonusData1, bonusData2, bonusData2);

        int result = sut.CalculateTeamBonus(champions);

        Assert.AreEqual(310, result);
    }

    [Test]
    public void è��_����_����_������_���ʽ�()
    {
        SortedDictionary<int, int> bonusData = new SortedDictionary<int, int>();
        bonusData.Add(300, 100);
        bonusData.Add(400, 150);
        var sut = new ChampionBonusCalculator(bonusData, bonusData);

        int result = sut.CalculateBonus(new Champion(300, 400, 0, 0));

        Assert.AreEqual(250, result);
    }

    [Test] 
    public void ���ʽ���_��������()
    {
        SortedDictionary<int, int> bonusData = new SortedDictionary<int, int>();
        bonusData.Add(300, 100);
        bonusData.Add(400, 150);
        var sut = new BonusCalculator(bonusData);

        Assert.AreEqual(100, sut.CalculateBonus(300));
        Assert.AreEqual(150, sut.CalculateBonus(400));
    }
}
