using NUnit.Framework;

public class PlayerMatchDataTests
{
    PlayerData CreateDummyPlayer(int id, string name = "Test") => new PlayerData(id, name, null);
    PlayerMatchData CreateMatchData(PlayerData p1, PlayerData p2) => new PlayerMatchData(p1, p2);

    [Test]
    public void ID를_전달하면_해당_플레이어_데이터를_찾는다()
    {
        var p1 = CreateDummyPlayer(10, "UserPlayer");
        var p2 = CreateDummyPlayer(20, "AIPlayer");
        var sut = CreateMatchData(p1, p2);

        Assert.AreEqual(p1, sut.GetPlayer(10));
        Assert.AreEqual(p2, sut.GetPlayer(20));
        Assert.IsNull(sut.GetPlayer(999)); // 없으면 null
    }

    [Test]
    public void 두_플레이어의_ID를_가진_MatchData를_반환한다()
    {
        var p1 = CreateDummyPlayer(10, "UserPlayer");
        var p2 = CreateDummyPlayer(20, "AIPlayer");
        var sut = CreateMatchData(p1, p2);

        var matchData = sut.ToMatchData();

        Assert.AreEqual(10, matchData.Id1);
        Assert.AreEqual(20, matchData.Id2);
    }

    [Test]
    public void 아이디를_입력하면_상대방의_데이터를_반환한다()
    {
        var player1 = CreateDummyPlayer(1);
        var player2 = CreateDummyPlayer(2);
        var matchData = CreateMatchData(player1, player2);

        var opponentOfPlayer1 = matchData.GetOpponent(1);
        var opponentOfPlayer2 = matchData.GetOpponent(2);

        Assert.AreEqual(player2, opponentOfPlayer1);
        Assert.AreEqual(player1, opponentOfPlayer2);
        Assert.IsNull(matchData.GetOpponent(0));
    }
}