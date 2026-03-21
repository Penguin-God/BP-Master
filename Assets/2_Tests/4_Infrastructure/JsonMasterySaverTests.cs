using NUnit.Framework;
using UnityEngine;

public class JsonMasterySaverTests
{
    const string TestKey = "Test_MasterySaveData";
    JsonMasterySaver CreateSut() => new JsonMasterySaver(TestKey);

    [TearDown]
    public void TearDown()
    {
        PlayerPrefs.DeleteKey(TestKey);
    }

    [Test]
    public void 인벤토리를_저장하고_로드하면_데이터가_정확히_복원된다()
    {
        var sut = CreateSut();

        var inventory = new MasteryProfile(startPoints: 10);
        inventory.Upgrade(101, StatType.Attack);
        inventory.Upgrade(102, StatType.Speed);

        sut.Save(inventory);
        var loadedInventory = sut.Load();

        Assert.AreEqual(8, loadedInventory.AvailablePoints);
        Assert.AreEqual(1, loadedInventory.GetBoard(101).AttackLevel);
        Assert.AreEqual(0, loadedInventory.GetBoard(101).SpeedLevel);
        Assert.AreEqual(1, loadedInventory.GetBoard(102).SpeedLevel);

        Assert.AreEqual(0, loadedInventory.GetBoard(103).AttackLevel);
    }

    [Test]
    public void 저장된_데이터가_없으면_Load는_null을_반환한다() => Assert.IsNull(CreateSut().Load());
}