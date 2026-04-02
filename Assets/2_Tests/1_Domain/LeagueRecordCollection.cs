using NUnit.Framework;

public class LeagueRecordCollectionTests
{
    LeagueRecordCollection CreateCollection(params LeagueRecord[] records) => new LeagueRecordCollection(records);

    [Test]
    public void Get은_존재하는_ID의_기록을_반환한다()
    {
        var collection = CreateCollection(new LeagueRecord(1, win: 10, lose: 5, score: 20));

        Assert.AreEqual(10, collection.Get(1).Win);
        Assert.AreEqual(1, collection.Get(1).Id);
        // 없는건 전부 0으로 초기화
        Assert.AreEqual(0, collection.Get(99).Win);
        Assert.AreEqual(99, collection.Get(99).Id);
    }

    [Test]
    public void Update는_기존_ID의_기록을_새로운_기록으로_덮어씌운다()
    {
        var collection = CreateCollection(new LeagueRecord(1, 0, 0, 333));
        var newRecord = new LeagueRecord(1, 2, 0, 4);

        collection.Update(newRecord);

        Assert.AreEqual(2, collection.Get(1).Win);
        Assert.AreEqual(4, collection.Get(1).Score);
    }

    [Test]
    public void 순위표는_승수_승점_ID오름_순으로_정렬된다()
    {
        var collection = CreateCollection(
            new LeagueRecord(id: 3, win: 5, lose: 0, score: 10),
            new LeagueRecord(id: 1, win: 5, lose: 2, score: 8),
            new LeagueRecord(id: 2, win: 5, lose: 2, score: 8), 
            new LeagueRecord(id: 4, win: 3, lose: 5, score: -2) 
        );

        var leaderboard = collection.GetSortedLeaderboard();

        Assert.AreEqual(3, leaderboard[0].Id); // 1위: 승수가 5이고 승점이 10으로 가장 높음
        Assert.AreEqual(1, leaderboard[1].Id); // 2위: 승수 5, 승점 8 (ID가 작아서 우선순위)
        Assert.AreEqual(2, leaderboard[2].Id); // 3위: 승수 5, 승점 8
        Assert.AreEqual(4, leaderboard[3].Id); // 4위: 승수가 3으로 가장 낮음
    }
}