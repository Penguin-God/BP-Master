using NUnit.Framework;

public class MatchFlowUsecaseTests
{
    [Test]
    [TestCase(Team.Blue, Team.Blue, 1, 0)] // 플레이어 블루, 블루 승리 -> 플레이어 1승
    [TestCase(Team.Blue, Team.Red, 0, 1)]  // 플레이어 블루, 레드 승리 -> AI 1승
    [TestCase(Team.Red, Team.Red, 1, 0)]   // 플레이어 레드, 레드 승리 -> 플레이어 1승
    public void 서비스는_결과를_받아_기록기의_점수를_정확히_올려야_함(Team playerSide, Team winnerSide, int expPlayer, int expAi)
    {
        var record = CreateRecord(2);
        var sut = CreateSut(record, playerSide);

        sut.EndMatch(winnerSide);

        Assert.AreEqual(expPlayer, record.PlayerWinCount);
        Assert.AreEqual(expAi, record.AiWinCount);
    }

    [Test]
    public void 매치_종료_조건_달성_시_기록기_상태가_변경되어야_함()
    {
        var record = CreateRecord(1);
        var sut = CreateSut(record, Team.Blue);

        sut.EndMatch(Team.Blue);

        Assert.IsTrue(record.IsMatchFinished);
        Assert.AreEqual(Participant.Player, record.MatchWinner);
    }

    // --- Helper Functions ---
    MatchRecord CreateRecord(int target) => new MatchRecord(target);
    MatchFlowUsecase CreateSut(MatchRecord record, Team playerSide) => new MatchFlowUsecase(record, playerSide);
}