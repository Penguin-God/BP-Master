using NUnit.Framework;
using Match;
using System.Collections.Generic;

public class MatchContextTests
{
    [TearDown]
    public void TearDown()
    {
        MatchContext.Clear();
    }

    PlayerMatchData CreatePlayerMatchData(int id1, int id2)
    {
        var p1 = new PlayerData(id1, "Player", new MasteryBoardCollection(new Dictionary<int, MasteryBoard>()));
        var p2 = new PlayerData(id2, "AI", new MasteryBoardCollection(new Dictionary<int, MasteryBoard>()));
        return new PlayerMatchData(p1, p2);
    }

    [Test]
    public void Init시_매치데이터_저장소_세팅()
    {
        var allIds = new[] { 10, 20, 30 };
        var playerMatchData = CreatePlayerMatchData(1, 100);

        MatchContext.MatchInit(playerMatchData, targetWin: 2, allIds);

        Assert.AreEqual(1, MatchContext.CurrentMatch.Id1);
        Assert.AreEqual(100, MatchContext.CurrentMatch.Id2);
        CollectionAssert.AreEqual(allIds, MatchContext.Storage.SelectableIds);
    }

    [Test]
    public void 게임이_끝나면_승리_기록_후_픽된_ID는_선택_풀에서_제외한다()
    {
        var allIds = new[] { 10, 20, 30 };
        var playerMatchData = CreatePlayerMatchData(1, 100);

        MatchContext.MatchInit(playerMatchData, 2, allIds);

        MatchContext.Storage.Pick(Team.Blue, 10);
        MatchContext.EndMatch(1);

        Assert.AreEqual(1, MatchContext.WinCounter.GetWin(1));
        CollectionAssert.AreEqual(new int[] { 20, 30 }, MatchContext.Storage.SelectableIds);
    }

    [Test]
    public void 승수를_채워서_매치가_끝나면_모든_상태를_비운다()
    {
        var playerMatchData = CreatePlayerMatchData(1, 100);

        MatchContext.MatchInit(playerMatchData, 2, new[] { 10 });

        Assert.IsFalse(MatchContext.EndMatch(1));
        Assert.IsTrue(MatchContext.EndMatch(1));

        Assert.AreEqual(0, MatchContext.CurrentMatch.Id1);
        Assert.IsNull(MatchContext.Storage);
        Assert.IsNull(MatchContext.WinCounter);
    }
}