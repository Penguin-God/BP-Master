using NUnit.Framework;

public class TraitActionTests
{
    [Test]
    [TestCase(10, 21)]
    [TestCase(-5, 6)]
    public void 챔피언상태_공_변경(int amount, int expected)
    {
        var target = TestHelper.CreateStatStatus(11, 0, 0);
        var sut = new AttackChanger(amount);

        sut.Do(target);

        Assert.AreEqual(expected, target.StatData.Attack);
    }

    [Test]
    [TestCase(5, 15)]
    [TestCase(-3, 7)]
    public void 챔피언상태_방_변경(int amount, int expected)
    {
        var target = TestHelper.CreateStatStatus(0, 10, 0);
        var sut = new DefenseChanger(amount);

        sut.Do(target);

        Assert.AreEqual(expected, target.StatData.Defense);
    }

    [Test]
    [TestCase(3, 10)]
    [TestCase(-2, 5)]
    public void 챔피언상태_속_변경(int amount, int expected)
    {
        var target = TestHelper.CreateStatStatus(0, 0, 7);
        var sut = new SpeedChanger(amount);

        sut.Do(target);

        Assert.AreEqual(expected, target.StatData.Speed);
    }
}
