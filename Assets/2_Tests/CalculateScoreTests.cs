using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CalculateScoreTests
{
    //[Test]
    //public void ���ʽ�_����_���()
    //{
    //    TeamScoreCalculator sut = new();
    //    Champion[] blue = null;
    //    Champion[] red = null;
    //    MatchResult result = sut.CalculateScore(blue, red);

    //    Assert.AreEqual(result);
    //}

    [Test]
    public void ����_����_���()
    {
        TeamScoreCalculator sut = new();
        Champion[] team = new Champion[] { new Champion(10, 30), new Champion(20, 50) };
        
        int result = sut.CalculateScore();

        Assert.AreEqual(110, result);
    }
}
