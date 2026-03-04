using NUnit.Framework;
using System;

public class MasteryInventoryTest
{
    MasteryInventory CreateInventory(int startPoints = 10)
    {
        var championIds = new[] { 101, 102 };
        return new MasteryInventory(championIds, startPoints);
    }

    [Test]
    public void 챔피언의_숙련도를_올리면_포인트가_감소하고_해당_보드의_스탯만이_증가한다()
    {
        var inventory = CreateInventory(startPoints: 10);

        inventory.Upgrade(101, StatType.Attack);

        Assert.AreEqual(9, inventory.AvailablePoints);
        Assert.AreEqual(1, inventory.GetBoard(101).AttackLevel);
        Assert.AreEqual(0, inventory.GetBoard(102).AttackLevel);
    }

    [Test]
    public void 포인트가_부족할_때_숙련도를_올리려_하면_예외를_던진다()
    {
        var inventory = CreateInventory(startPoints: 0);

        Assert.Throws<InvalidOperationException>(() => inventory.Upgrade(101, StatType.Attack));
    }

    [Test]
    public void 보드의_최대_레벨에_도달하여_실패하면_포인트는_감소하지_않는다()
    {
        var inventory = CreateInventory(startPoints: 10);
        inventory.Upgrade(101, StatType.Speed); // 1레벨 달성

        Assert.Throws<InvalidOperationException>(() => inventory.Upgrade(101, StatType.Speed));
        Assert.AreEqual(9, inventory.AvailablePoints);
    }
}