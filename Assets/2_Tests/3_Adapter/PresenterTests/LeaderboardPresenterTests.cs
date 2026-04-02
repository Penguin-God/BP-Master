using NUnit.Framework;

public class LeaderboardPresenterTests
{
    [Test]
    public void 순위대로_정렬_후_모델로_변환해_반환된다()
    {
        var records = new[]
        {
            new LeagueRecord(id: 2, win: 1, lose: 2, score: -1), // 2등
            new LeagueRecord(id: 1, win: 5, lose: 0, score: 10), // 1등
            new LeagueRecord(id: 4, win: 0, lose: 2, score: 0) // 3등
        };
        var collection = new LeagueRecordCollection(records);
        var presenter = CreateSut(collection, new FakePlayerDataLoader());

        var displayData = presenter.GetDisplayData();

        Assert.AreEqual(3, displayData.Count);

        Assert.AreEqual(1, displayData[0].Rank);
        Assert.AreEqual("Team_1", displayData[0].TeamName);
        Assert.AreEqual("5", displayData[0].WinText);
        Assert.AreEqual("0", displayData[0].LoseText);
        Assert.AreEqual("+10", displayData[0].ScoreText); // 양수 기호 확인

        Assert.AreEqual(2, displayData[1].Rank);
        Assert.AreEqual("Team_2", displayData[1].TeamName);
        Assert.AreEqual("1", displayData[1].WinText);
        Assert.AreEqual("2", displayData[1].LoseText);
        Assert.AreEqual("-1", displayData[1].ScoreText); // 음수 기호 확인

        Assert.AreEqual("0", displayData[2].ScoreText); // 0일 경우 기호 없음 확인
    }

    LeaderboardPresenter CreateSut(LeagueRecordCollection collection, IPlayerDataLoader loader) => new LeaderboardPresenter(collection, loader);

    class FakePlayerDataLoader : IPlayerDataLoader
    {
        public PlayerData LoadPlayer(int id) => new PlayerData(id, $"Team_{id}", null);
    }
}