using NUnit.Framework;

public class StatChangeTests
{
    [Test]
    public void 생성자에서_음수값은_0으로_보정된다()
    {
        var sut = new ChampionStatData(-5, -1, -100);

        Assert.AreEqual(0, sut.Attack);
        Assert.AreEqual(0, sut.Defense);
        Assert.AreEqual(0, sut.Speed);
    }

    [Test]
    public void 공격력만_변경()
    {
        var sut = new ChampionStatData(3, 4, 5);

        var changed = sut.ChangeAttack(10);

        Assert.AreEqual(10, changed.Attack);
        Assert.AreEqual(4, changed.Defense);
        Assert.AreEqual(5, changed.Speed);
    }

    [Test]
    public void 방어력만_변경()
    {
        var sut = new ChampionStatData(3, 4, 5);

        var changed = sut.ChangeDefense(20);

        Assert.AreEqual(3, changed.Attack);
        Assert.AreEqual(20, changed.Defense);
        Assert.AreEqual(5, changed.Speed);
    }

    [Test]
    public void 속도만_변경()
    {
        var sut = new ChampionStatData(3, 4, 5);

        var changed = sut.ChangeSpeed(30);

        Assert.AreEqual(3, changed.Attack);
        Assert.AreEqual(4, changed.Defense);
        Assert.AreEqual(30, changed.Speed);
    }

    [Test]
    public void 변경_시_음수값은_0으로_보정한다()
    {
        var sut = new ChampionStatData(3, 4, 5);

        var changedAttack = sut.ChangeAttack(-10);
        Assert.AreEqual(0, changedAttack.Attack);

        var changedDefense = sut.ChangeDefense(-20);
        Assert.AreEqual(0, changedDefense.Defense);

        var changedSpeed = sut.ChangeSpeed(-30);
        Assert.AreEqual(0, changedSpeed.Speed);
    }

    [Test]
    public void 스탯끼리_더하면_각_항목이_합산된다()
    {
        var stat1 = new ChampionStatData(attack: 10, defense: 5, speed: 2);
        var stat2 = new ChampionStatData(attack: 15, defense: 10, speed: 3);

        var result = stat1 + stat2;

        Assert.AreEqual(25, result.Attack);
        Assert.AreEqual(15, result.Defense);
        Assert.AreEqual(5, result.Speed);
    }
}
