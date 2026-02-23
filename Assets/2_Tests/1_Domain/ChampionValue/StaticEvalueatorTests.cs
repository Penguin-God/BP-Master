using NUnit.Framework;

public class StaticEvalueatorTests
{
    [Test]
    public void 팀에_따라_아군_적군_합해서_밸류_계산()
    {
        var sut = new ChampionStatValueCalculator(10);
        var data = new GameScoreInfo(new ScoreInfo(100, 100, 5), new ScoreInfo(-500, 300, 0));

        int result = sut.CalcualteTeamStatValue(data, Team.Blue);

        // 250 + 200 = 450
        Assert.AreEqual(450, result);
    }
}
