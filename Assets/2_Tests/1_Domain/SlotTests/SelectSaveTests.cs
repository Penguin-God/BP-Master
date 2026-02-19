using NUnit.Framework;
using static TestHelper;

public class SelectSaveTests
{
    [Test]
    public void 중복_선택_불가()
    {
        const int Id = 3;
        BanPickStorage storage = CreateStorage(Id);
        Select(storage, Team.Blue, GamePhase.Pick, Id);
        Assert.IsFalse(Select(storage, Team.Blue, GamePhase.Pick, Id));
    }

    [Test]
    public void Id_저장_후_저장한_슬롯_위치_반환()
    {
        var sut = CreateStorage(3, 4, 5);
        Assert.AreEqual(sut.Pick(Team.Blue, 3), BlueZeroSlot);
        Assert.AreEqual(sut.Pick(Team.Blue, 4), BlueOneSlot);
        Assert.AreEqual(sut.Pick(Team.Red, 5), RedZeroSlot);

        Assert.AreEqual(sut.PickIds.GetSlot(BlueZeroSlot), 3);
        Assert.AreEqual(sut.PickIds.GetSlot(BlueOneSlot), 4);
        Assert.AreEqual(sut.PickIds.GetSlot(RedZeroSlot), 5);
    }

    [Test]
    public void 밴픽에_따른_이벤트()
    {
        var storage = CreateStorage(11, 22, 101, 102, 201);

        int ban = 0;
        
        storage.OnBan += (team, id) => ban = id;

        Select(storage, Team.Blue, GamePhase.Ban, 201);

        Assert.AreEqual(201, ban);
    }

    bool Select(BanPickStorage storage, Team team, GamePhase phase, int id) => storage.SaveSelect(CreateFlow(phase, team), id);
}
