using NUnit.Framework;
using Match;

public class MatchContextTests
{
    [TearDown]
    public void TearDown()
    {
        MatchContext.Clear();
    }

    MatchData CreateMatchData(int id1, int id2) => new MatchData(id1, id2);

    [Test]
    public void Init시_매치데이터_저장소_세팅()
    {
        var allIds = new[] { 10, 20, 30 };
        var playerMatchData = CreateMatchData(1, 100);

        MatchContext.MatchInit(playerMatchData, targetWin: 2, allIds);

        Assert.AreEqual(1, MatchContext.CurrentMatch.Id1);
        Assert.AreEqual(100, MatchContext.CurrentMatch.Id2);
        CollectionAssert.AreEqual(allIds, MatchContext.Storage.SelectableIds);
    }

    [Test]
    public void 피어리스에_포함된_id는_저장소에서_제외된다()
    {
        var allIds = new[] { 10, 20, 30 };
        var playerMatchData = CreateMatchData(1, 100);

        MatchContext.MatchInit(playerMatchData, targetWin: 2, allIds);
        MatchContext.RecordMatchResult(new int[] { 10 });

        var result = MatchContext.CreateFearlessStorage().SelectableIds;

        CollectionAssert.DoesNotContain(result, 10);
    }

    [Test]
    public void 승수를_채워서_매치가_끝나면_모든_상태를_비운다()
    {
        var playerMatchData = CreateMatchData(1, 100);

        MatchContext.MatchInit(playerMatchData, 2, new[] { 10 });

        Assert.IsFalse(MatchContext.EndMatch(1));
        Assert.IsTrue(MatchContext.EndMatch(1));

        Assert.AreEqual(0, MatchContext.CurrentMatch.Id1);
        Assert.IsNull(MatchContext.Storage);
        Assert.IsNull(MatchContext.WinCounter);
    }
}