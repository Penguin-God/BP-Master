using NUnit.Framework;

public class MatchRecordTests
{
    [Test]
    [TestCase(Team.Blue, 1, 0)]
    [TestCase(Team.Red, 0, 1)]
    public void 승리_기록_시_해당_팀의_점수만_올라가야_함(Team winner, int expectedBlue, int expectedRed)
    {
        var sut = CreateMatchRecord(2);

        sut.AddWin(winner);

        Assert.AreEqual(expectedBlue, sut.BlueWins);
        Assert.AreEqual(expectedRed, sut.RedWins);
    }

    [Test]
    public void 목표_승수_도달_시_매치가_종료되어야_함()
    {
        var sut = CreateMatchRecord(2);

        sut.AddWin(Team.Red);
        sut.AddWin(Team.Red);

        Assert.IsTrue(sut.IsMatchFinished);
        Assert.AreEqual(Team.Red, sut.MatchWinner);
    }

    private MatchRecord CreateMatchRecord(int targetWins) => new MatchRecord(targetWins);
}