using NUnit.Framework;

public class MatchRecordTests
{
    [Test]
    [TestCase(Participant.Player, 1, 0)]
    [TestCase(Participant.AI, 0, 1)]
    public void 주체별_승리_기록_테스트(Participant winner, int expectedPlayer, int expectedAi)
    {
        // Arrange
        var sut = new MatchRecord(2);

        // Act
        sut.AddWin(winner);

        // Assert
        Assert.AreEqual(expectedPlayer, sut.PlayerWins);
        Assert.AreEqual(expectedAi, sut.AiWins);
    }

    [Test]
    public void 플레이어_최종_우승_판정()
    {
        // Arrange
        var sut = new MatchRecord(2);

        // Act
        sut.AddWin(Participant.Player);
        sut.AddWin(Participant.Player);

        // Assert
        Assert.IsTrue(sut.IsMatchFinished);
        Assert.AreEqual(Participant.Player, sut.MatchWinner);
    }

    private MatchRecord CreateMatchRecord(int targetWins) => new MatchRecord(targetWins);
}