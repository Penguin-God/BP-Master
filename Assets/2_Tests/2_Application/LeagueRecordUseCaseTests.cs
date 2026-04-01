using NUnit.Framework;
using System.Collections.Generic;

public class LeagueRecordUseCaseTests
{
    [Test]
    public void 기존_기록이_있다면_누적하고_없다면_새로_저장한다()
    {
        var storage = CreateDummyStorage();
        storage.SaveAll(new Dictionary<int, LeagueRecord>
        {
            { 1, new LeagueRecord(1, 0, 2) }
        });
        var usecase = CreateSut(storage);

        usecase.RecordMatch(player1Id: 1, 2, player2Id: 2, 0);

        var result = storage.LoadAll();
        Assert.AreEqual(2, result[1].Win);
        Assert.AreEqual(4, result[1].Score);

        Assert.AreEqual(0, result[2].Win);
        Assert.AreEqual(1, result[2].Lose);
        Assert.AreEqual(-2, result[2].Score);
    }

    LeagueRecordUseCase CreateSut(ILeagueRecordStorage storage) => new LeagueRecordUseCase(storage);
    DummyStorage CreateDummyStorage() => new DummyStorage();

    class DummyStorage : ILeagueRecordStorage
    {
        Dictionary<int, LeagueRecord> data = new Dictionary<int, LeagueRecord>();

        public Dictionary<int, LeagueRecord> LoadAll() => data;

        public void SaveAll(Dictionary<int, LeagueRecord> records) => data = records;
    }
}