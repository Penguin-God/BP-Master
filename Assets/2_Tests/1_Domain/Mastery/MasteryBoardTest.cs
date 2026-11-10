using NUnit.Framework;

public class MasteryBoardTest
{
    MasteryBoard CreateSut() => new MasteryBoard();

    [Test]
    public void 스탯별_숙련도를_올리면_레벨이_오르고_true를_반환한다()
    {
        var sut = CreateSut();

        Assert.IsTrue(sut.Upgrade(StatType.Attack));
        Assert.AreEqual(1, sut.AttackLevel);
        Assert.AreEqual(0, sut.DefenseLevel);

        Assert.IsTrue(sut.Upgrade(StatType.Defense));
        Assert.AreEqual(1, sut.DefenseLevel);
        Assert.IsTrue(sut.IsUpgrade);
    }

    [Test]
    public void 스탯별_숙련도를_내리면_레벨이_내려가고_true를_반환한다()
    {
        var sut = new MasteryBoard(attackLevel: 1, defenseLevel: 1);

        Assert.IsTrue(sut.Downgrade(StatType.Attack));
        Assert.AreEqual(0, sut.AttackLevel);
        Assert.AreEqual(1, sut.DefenseLevel);

        Assert.IsTrue(sut.Downgrade(StatType.Defense));
        Assert.AreEqual(0, sut.DefenseLevel);
    }

    [Test]
    public void 최대_레벨에서_숙련도를_올리면_레벨이_유지되고_false를_반환한다()
    {
        var sut = new MasteryBoard(attackLevel: 1);

        bool isChanged = sut.Upgrade(StatType.Attack);

        Assert.IsFalse(isChanged);
        Assert.AreEqual(1, sut.AttackLevel);
    }

    [Test]
    public void 최소_레벨에서_숙련도를_내리면_레벨이_유지되고_false를_반환한다()
    {
        var sut = CreateSut();

        bool isChanged = sut.Downgrade(StatType.Attack);

        Assert.IsFalse(isChanged);
        Assert.AreEqual(0, sut.AttackLevel);
        Assert.IsFalse(sut.IsUpgrade);
    }
}
