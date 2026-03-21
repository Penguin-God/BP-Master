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
    public void 포인트가_부족할_때_숙련도를_올리려_하면_예외를_던진다()
    {
        var inventory = CreateMasteryInventory(0);

        Assert.Throws<InvalidOperationException>(() => inventory.Upgrade(101, StatType.Attack));
    }

    [Test]
    public void 보드의_최대_레벨에_도달하여_실패하면_포인트는_감소하지_않는다()
    {
        var inventory = CreateMasteryInventory(10);
        inventory.Upgrade(101, StatType.Speed); // 1레벨 달성 (스피드 최대 레벨이 1이라고 가정)

        Assert.Throws<InvalidOperationException>(() => inventory.Upgrade(101, StatType.Speed));
        Assert.AreEqual(9, inventory.AvailablePoints);
    }
}