using NUnit.Framework;
using System;
using static TestHelper;

public class MasteryInventoryTest
{
    [Test]
    public void 챔피언의_숙련도를_올리면_포인트가_감소하고_새로운_보드가_생성되어_스탯이_증가한다()
    {
        var inventory = CreateMasteryInventory(10);

        inventory.Upgrade(101, StatType.Attack);

        Assert.AreEqual(9, inventory.AvailablePoints);
        Assert.AreEqual(1, inventory.GetBoard(101).AttackLevel);

        Assert.AreEqual(0, inventory.GetBoard(102).AttackLevel);
    }

    [Test]
    public void 카드를_다운그레이드하면_포인트가_증가한다()
    {
        var inventory = CreateMasteryInventory(10);

        inventory.Upgrade(101, StatType.Attack);
        inventory.Downgrade(101, StatType.Attack);

        Assert.AreEqual(10, inventory.AvailablePoints);
        Assert.AreEqual(0, inventory.GetBoard(101).AttackLevel);
    }

    [Test]
    public void 보드의_최대_레벨에_도달하여_실패하면_포인트는_감소하지_않는다()
    {
        var inventory = CreateMasteryInventory(10);
        inventory.Upgrade(101, StatType.Speed); // 1레벨 달성 (스피드 최대 레벨이 1이라고 가정)

        Assert.AreEqual(9, inventory.AvailablePoints);
    }

    [Test]
    public void 포인트를_획득하면_총량과_가용포인트가_모두_증가한다()
    {
        var inventory = CreateMasteryInventory(10);

        inventory.EarnPoints(5);

        Assert.AreEqual(15, inventory.TotalPoints);
        Assert.AreEqual(15, inventory.AvailablePoints);
    }

    [Test]
    public void 초기화를_요청하면_보드가_비워지고_포인트가_총량만큼_복구된다()
    {
        var inventory = CreateMasteryInventory(10);
        inventory.Upgrade(101, StatType.Attack);
        inventory.Upgrade(102, StatType.Defense);

        Assert.AreEqual(8, inventory.AvailablePoints);

        inventory.ResetAll();

        Assert.AreEqual(10, inventory.TotalPoints);
        Assert.AreEqual(10, inventory.AvailablePoints);
        Assert.AreEqual(0, inventory.GetBoard(101).AttackLevel);
    }
}