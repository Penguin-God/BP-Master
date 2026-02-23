using static TestHelper;
using NUnit.Framework;
using System.Collections.Generic;

public class BonusDeltaCalculateTests
{
    [Test]
    public void 보너스_오르는거_계산하기()
    {
        TeamBonusCalculator teamBonusCalculator = new TeamBonusCalculator(new BonusCalculator(new SortedDictionary<int, int>() { { 100, 100 }, { 200, 200 } }), CreateBonus(0, 0), CreateBonus(10, 100));
        var sut = new BonusDeltaCalculator(teamBonusCalculator);

        var before = new GameScoreInfo(new ScoreInfo(100, 100, 0), new ScoreInfo(100, 100, 0));
        var after = new GameScoreInfo(new ScoreInfo(200, 100, 10), new ScoreInfo(0, 100, 0));

        int result = sut.Calculate(before, after, Team.Blue);

        // blue 200 증가, red 100감소. blue입장에서 300 이득
        Assert.AreEqual(300, result);
    }
}
