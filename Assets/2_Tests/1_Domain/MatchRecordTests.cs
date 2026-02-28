using NUnit.Framework;

public class MatchRecordTests
{
    [Test]
    [TestCase(Participant.Player, 1, 0)]
    [TestCase(Participant.AI, 0, 1)]
    public void 주체별_승리_기록_테스트(Participant winner, int expectedPlayer, int expectedAi)
    {
        var sut = CreateSut(2);

        sut.AddWin(winner);

        Assert.AreEqual(expectedPlayer, sut.PlayerWinCount);
        Assert.AreEqual(expectedAi, sut.AiWinCount);
    }

    [Test]
    public void AI_최종_우승_판정()
    {
        var sut = CreateSut(2);

        sut.AddWin(Participant.AI);
        sut.AddWin(Participant.AI);

        Assert.IsTrue(sut.IsMatchFinished);
        Assert.AreEqual(Participant.AI, sut.MatchWinner);
    }

    private MatchRecord CreateSut(int targetWins) => new MatchRecord(targetWins);

    public void Id에_따라_승리_기록()
    {
        var sut = new MatchWinCounter(new MatchData(1, 2), 2);

        sut.AddWin(1);

        Assert.AreEqual(sut.GetWin(1), 1);
        Assert.AreEqual(sut.GetWin(2), 0);
    }

    public void 승수를_채운_최초의_ID가_등장하면_매치_끝()
    {
        var sut = new MatchWinCounter(new MatchData(1, 2), 2);

        sut.AddWin(1);
        Assert.AreEqual(sut.IsMatchFinished, false);

        sut.AddWin(1);
        Assert.AreEqual(sut.IsMatchFinished, true);
    }
}