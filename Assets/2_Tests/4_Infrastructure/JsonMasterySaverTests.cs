using NUnit.Framework;
using UnityEngine;

public class JsonMasterySaverTests
{
    const string TestKey = "Test_MasterySaveData"; // 실제 게임 데이터와 겹치지 않는 테스트 전용 키

    JsonMasterySaver CreateSut()
    {
        return new JsonMasterySaver(TestKey);
    }

    MasteryInventory CreateInventory()
    {
        var inventory = new MasteryInventory(new[] { 101, 102 }, 10);
        inventory.Upgrade(101, StatType.Attack);
        inventory.Upgrade(102, StatType.Speed);
        return inventory;
    }

    [TearDown]
    public void TearDown()
    {
        PlayerPrefs.DeleteKey(TestKey);
    }

    [Test]
    public void 인벤토리를_저장하고_로드하면_데이터가_정확히_복원된다()
    {
        var sut = CreateSut();
        var originalInventory = CreateInventory();

        sut.Save(originalInventory);
        var loadedInventory = sut.Load();

        Assert.AreEqual(8, loadedInventory.AvailablePoints);
        Assert.AreEqual(1, loadedInventory.GetBoard(101).AttackLevel);
        Assert.AreEqual(0, loadedInventory.GetBoard(101).SpeedLevel);
        Assert.AreEqual(1, loadedInventory.GetBoard(102).SpeedLevel);
    }

    [Test]
    public void 저장된_데이터가_없으면_Load는_null을_반환한다()
    {
        var sut = CreateSut();

        var loadedInventory = sut.Load();

        Assert.IsNull(loadedInventory);
    }
}