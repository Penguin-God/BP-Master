using NUnit.Framework;
using System.Collections.Generic;

public class LeagueRecordUseCaseTests
{
    [Test]
    public void 기존_기록이_있다면_누적하고_없다면_새로_저장한다()
    {
        var storage = CreateDummyStorage();
        
        var initialData = new List<LeagueRecord> { new LeagueRecord(id: 1, win: 1, lose: 0, score: 2) };
        storage.SaveAll(new LeagueRecordCollection(initialData));
        
        var usecase = CreateSut(storage);

        var resultCollection = usecase.RecordMatch(player1Id: 1, 2, player2Id: 2, 0);
        
        var p1Record = resultCollection.Get(1);
        var p2Record = resultCollection.Get(2);

        Assert.AreEqual(2, p1Record.Win);
        Assert.AreEqual(4, p1Record.Score);

        Assert.AreEqual(0, p2Record.Win);
        Assert.AreEqual(1, p2Record.Lose);
        Assert.AreEqual(-2, p2Record.Score);
    }

    LeagueRecordUseCase CreateSut(ILeagueRecordStorage storage) => new LeagueRecordUseCase(storage);
    DummyStorage CreateDummyStorage() => new DummyStorage();

    class DummyStorage : ILeagueRecordStorage
    {
        LeagueRecordCollection data = new LeagueRecordCollection(new List<LeagueRecord>());

        public LeagueRecordCollection LoadAll() => data;
        public void SaveAll(LeagueRecordCollection collection) => data = collection;
    }
}