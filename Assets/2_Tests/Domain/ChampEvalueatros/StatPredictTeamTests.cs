using NUnit.Framework;
using static TestHelper;

public class StatPredictTeamTests
{
    [Test]
    public void 챔피언의_평균_스탯_반환()
    {
        var stats = new ChampionStatData[] { CreateStat(0, 0), CreateStat(100, 100) };
        var sut = new ChampionStatAverager(stats);

        ChampionStatData result = sut.GetStatAverage();

        Assert.AreEqual(50, result.Attack);
        Assert.AreEqual(50, result.Defense);
    }
}
