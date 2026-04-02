using NUnit.Framework;

public class LeagueRecordTests
{
    [TestCase(2, 0, 1, 0, 2)]
    [TestCase(2, 1, 1, 0, 1)]
    [TestCase(1, 2, 0, 1, -1)]
    public void 매치_결과에_따라_승패와_승점이_추가된_새_기록을_반환한다(int myWins, int opponentWins, int expectedWin, int expectedLose, int expectedScore)
    {
        var record = CreateSut(0, 0, 0);

        var newRecord = record.ApplyMatchResult(myWins, opponentWins);

        Assert.AreEqual(expectedWin, newRecord.Win);
        Assert.AreEqual(expectedLose, newRecord.Lose);
        Assert.AreEqual(expectedScore, newRecord.Score);
    }

    [Test]
    public void 기존_상태는_변경되지_않는다()
    {
        var record = CreateSut(id: 1, 1, 1, 1);

        record.ApplyMatchResult(2, 0);

        Assert.AreEqual(1, record.Win);
        Assert.AreEqual(1, record.Lose);
        Assert.AreEqual(1, record.Score);
    }

    LeagueRecord CreateSut(int id = 0, int win = 0, int loss = 0, int soore = 0) => new LeagueRecord(id, win, loss, soore);
}