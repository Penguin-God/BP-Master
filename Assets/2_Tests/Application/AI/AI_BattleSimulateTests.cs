using NUnit.Framework;

public class AI_BattleSimulateTests
{
    [Test]
    public void 주입한_AI로_게임_진행_후_결과_반환()
    {
        var sut = new AI_BattleSimulator(null, null);
        var blue = new AI_Agent(null, null);
        var red = new AI_Agent(null, null);

        var result = sut.Run(blue, red);

        Assert.AreEqual(Team.Blue, result.Winner);
    }
}
