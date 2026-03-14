using NUnit.Framework;

public class PlayerMatchDataTests
{
    PlayerData CreateDummyPlayer(int id, string name) => new PlayerData(id, name, null);

    [Test]
    public void ID를_전달하면_해당_플레이어_데이터를_찾는다()
    {
        var p1 = CreateDummyPlayer(10, "UserPlayer");
        var p2 = CreateDummyPlayer(20, "AIPlayer");
        var sut = new PlayerMatchData(p1, p2);

        Assert.AreEqual(p1, sut.GetPlayer(10));
        Assert.AreEqual(p2, sut.GetPlayer(20));
        Assert.IsNull(sut.GetPlayer(999)); // 없으면 null
    }

    [Test]
    public void 두_플레이어의_ID를_가진_MatchData를_반환한다()
    {
        var p1 = CreateDummyPlayer(10, "UserPlayer");
        var p2 = CreateDummyPlayer(20, "AIPlayer");
        var sut = new PlayerMatchData(p1, p2);

        var matchData = sut.ToMatchData();

        Assert.AreEqual(10, matchData.Id1);
        Assert.AreEqual(20, matchData.Id2);
    }
}