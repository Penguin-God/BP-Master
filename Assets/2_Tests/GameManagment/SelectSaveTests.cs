using NUnit.Framework;

public class SelectSaveTests
{
    [Test]
    public void 중복_선택_불가()
    {
        const int Id = 3;
        GameBanPickStorage storage = CreateStorage(Id);

        Assert.IsTrue(Select(storage, Team.Blue, SelectType.Ban, Id));
        Assert.IsFalse(Select(storage, Team.Blue, SelectType.Ban, Id));
        Assert.IsFalse(Select(storage, Team.Red, SelectType.Pick, Id));
        CollectionAssert.DoesNotContain(storage.SelectableIds, Id);
    }

    [Test]
    public void 교차_선택_시_팀별_저장_보장()
    {
        var storage = CreateStorage(101, 102, 201, 202);

        Select(storage, Team.Red, SelectType.Pick, 101);
        Select(storage, Team.Red, SelectType.Pick, 102);
        Select(storage, Team.Blue, SelectType.Pick, 201);
        Select(storage, Team.Blue, SelectType.Pick, 202);

        var redList = storage.GetStorage(Team.Red, SelectType.Pick);
        var blueList = storage.GetStorage(Team.Blue, SelectType.Pick);

        CollectionAssert.AreEqual(new[] { 101, 102 }, redList);
        CollectionAssert.AreEqual(new[] { 201, 202 }, blueList);
    }


    [Test]
    public void 밴픽_순서_섞여도_간섭없음()
    {
        var storage = CreateStorage(11, 22, 101, 102, 201);

        Select(storage, Team.Red, SelectType.Ban, 11);
        Select(storage, Team.Red, SelectType.Pick, 101);
        Select(storage, Team.Red, SelectType.Pick, 102);
        Select(storage, Team.Blue, SelectType.Ban, 22);
        Select(storage, Team.Blue, SelectType.Pick, 201);

        CollectionAssert.AreEqual(new[] { 11 }, storage.GetStorage(Team.Red, SelectType.Ban));
        CollectionAssert.AreEqual(new[] { 22 }, storage.GetStorage(Team.Blue, SelectType.Ban));
        CollectionAssert.AreEqual(new[] { 101, 102 }, storage.GetStorage(Team.Red, SelectType.Pick));
        CollectionAssert.AreEqual(new[] { 201 }, storage.GetStorage(Team.Blue, SelectType.Pick));
    }

    [Test]
    public void 픽_챔피언_반환()
    {
        var storage = CreateStorage(11, 22, 101, 102, 201);
        Select(storage, Team.Blue, SelectType.Ban, 201);
        Select(storage, Team.Blue, SelectType.Pick, 11);
        Select(storage, Team.Blue, SelectType.Pick, 22);
        Select(storage, Team.Red, SelectType.Pick, 101);
        Select(storage, Team.Red, SelectType.Pick, 102);

        var result = storage.PickBySlot;

        Assert.AreEqual(11, result[TestHelper.CreateBlueSlot(0)]);
        Assert.AreEqual(22, result[TestHelper.CreateBlueSlot(1)]);
        Assert.AreEqual(101, result[TestHelper.CreateRedSlot(0)]);
        Assert.AreEqual(102, result[TestHelper.CreateRedSlot(1)]);
    }

    [Test]
    public void 밴픽에_따른_이벤트()
    {
        var storage = CreateStorage(11, 22, 101, 102, 201);

        int ban = 0;
        int pick = 0;

        storage.OnBan += (team, id) => ban = id;
        storage.OnPick += (team, id) => pick = id;

        Select(storage, Team.Blue, SelectType.Ban, 201);
        Select(storage, Team.Blue, SelectType.Pick, 11);

        Assert.AreEqual(201, ban);
        Assert.AreEqual(11, pick);
    }

    bool Select(GameBanPickStorage storage, Team team, SelectType select, int id) => storage.SaveSelect(new SelectInfo(team, select, id));
    GameBanPickStorage CreateStorage(params int[] selectableIds) => new GameBanPickStorage(selectableIds);
}
