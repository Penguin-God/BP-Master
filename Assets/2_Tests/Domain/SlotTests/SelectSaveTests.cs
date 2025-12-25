using NUnit.Framework;
using System;
using static TestHelper;

public class SelectSaveTests
{
    [Test]
    public void 중복_선택_불가()
    {
        const int Id = 3;
        GameBanPickStorage storage = CreateStorage(Id);
        Select(storage, Team.Blue, GamePhase.Ban, Id);

        Assert.Throws<Exception>(() => Select(storage, Team.Blue, GamePhase.Done, 1));
    }

    [Test]
    public void 밴픽이_아닌_페이즈는_선택_불가()
    {
        GameBanPickStorage storage = CreateStorage(1);
        Assert.Throws<Exception>(() => Select(storage, Team.Blue, GamePhase.Done, 1));
    }

    [Test]
    public void 밴픽에_따른_이벤트()
    {
        var storage = CreateStorage(11, 22, 101, 102, 201);

        int ban = 0;
        SlotData pickSlot = default;
        int pick = 0;

        storage.OnBan += (team, id) => ban = id;
        storage.OnPick += (SlotData, id) => (pickSlot, pick) = (SlotData, id);

        Select(storage, Team.Blue, GamePhase.Ban, 201);
        Select(storage, Team.Blue, GamePhase.Pick, 11);
        Select(storage, Team.Blue, GamePhase.Pick, 101);

        Assert.AreEqual(201, ban);
        Assert.AreEqual(BlueOneSlot, pickSlot);
        Assert.AreEqual(101, pick);
    }

    void Select(GameBanPickStorage storage, Team team, GamePhase phase, int id) => storage.SaveSelect(CreateFlow(phase, team), id);
}
