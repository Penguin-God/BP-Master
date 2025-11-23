using NUnit.Framework;
using static TestHelper;

public class SelectSaveTests
{
    [Test]
    public void 중복_선택_불가()
    {
        const int Id = 3;
        GameBanPickStorage storage = CreateStorage(Id);

        Assert.IsTrue(storage.CanSelected(Id));
        Select(storage, Team.Blue, SelectType.Ban, Id);
        Assert.IsFalse(storage.CanSelected(Id));
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

        Select(storage, Team.Blue, SelectType.Ban, 201);
        Select(storage, Team.Blue, SelectType.Pick, 11);
        Select(storage, Team.Blue, SelectType.Pick, 101);

        Assert.AreEqual(201, ban);
        Assert.AreEqual(TestHelper.BlueOneSlot, pickSlot);
        Assert.AreEqual(101, pick);
    }

    void Select(GameBanPickStorage storage, Team team, SelectType select, int id) => storage.SaveSelect(new SelectInfo(team, select, id));
}
