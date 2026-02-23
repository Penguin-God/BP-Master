using NUnit.Framework;

public class StaticEvalueatorTests
{
    [Test]
    public void 팀에_따라_아군_적군_합해서_밸류_계산()
    {
        var sut = new ChampionStatValueCalculator(10);
        var data = new GameScoreInfo(new ScoreInfo(100, 100, 5), new ScoreInfo(-500, 300, 0));

        int result = sut.CalcualteTeamStatValue(data, Team.Blue);

        // (100 + 100 + 50) = 250
        // -500 +300 = -200을 상대팀이니까 +로 변환
        Assert.AreEqual(450, result);
    }
}
