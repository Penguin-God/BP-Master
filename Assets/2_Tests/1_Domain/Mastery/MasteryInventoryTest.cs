using NUnit.Framework;
using static TestHelper;

public class MasteryInventoryTest
{
    [Test]
    public void 챔피언의_숙련도를_올리면_포인트가_감소하고_새로운_보드가_생성되어_스탯이_증가한다()
    {
        var sut = CreateMasteryInventory(10);

        sut.Upgrade(101, StatType.Attack);

        Assert.AreEqual(9, sut.AvailablePoints);
        Assert.AreEqual(1, sut.GetBoard(101).AttackLevel);

        Assert.AreEqual(0, sut.GetBoard(102).AttackLevel);
    }

    [Test]
    public void 카드를_다운그레이드하면_포인트가_증가한다()
    {
        var sut = CreateMasteryInventory(10);

        sut.Upgrade(101, StatType.Attack);
        sut.Downgrade(101, StatType.Attack);

        Assert.AreEqual(10, sut.AvailablePoints);
        Assert.AreEqual(0, sut.GetBoard(101).AttackLevel);
    }

    [Test]
    public void 보드의_최대_레벨에_도달하여_실패하면_포인트는_감소하지_않는다()
    {
        var sut = CreateMasteryInventory(10);
        sut.Upgrade(101, StatType.Defense);
        sut.Upgrade(101, StatType.Defense);

        Assert.AreEqual(9, sut.AvailablePoints);
    }

    [Test]
    public void 보드의_최소_레벨이여서_다운그레이드_실패시__포인트는_증가하지_않는다()
    {
        var sut = CreateMasteryInventory(10);
        sut.Downgrade(101, StatType.Defense);

        Assert.AreEqual(10, sut.AvailablePoints);
    }

    [Test]
    public void 포인트를_획득하면_총량과_가용포인트가_모두_증가한다()
    {
        var sut = CreateMasteryInventory(10);

        sut.EarnPoints(5);

        Assert.AreEqual(15, sut.TotalPoints);
        Assert.AreEqual(15, sut.AvailablePoints);
    }

    [Test]
    public void 초기화를_요청하면_보드가_비워지고_포인트가_총량만큼_복구된다()
    {
        var sut = CreateMasteryInventory(10);
        sut.Upgrade(101, StatType.Attack);
        sut.Upgrade(102, StatType.Defense);

        Assert.AreEqual(8, sut.AvailablePoints);

        sut.ResetAll();

        Assert.AreEqual(10, sut.TotalPoints);
        Assert.AreEqual(10, sut.AvailablePoints);
        Assert.AreEqual(0, sut.GetBoard(101).AttackLevel);
    }
}
