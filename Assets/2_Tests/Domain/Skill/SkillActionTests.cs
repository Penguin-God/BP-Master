using NUnit.Framework;

public class TraitActionTests
{
    [Test]
    [TestCase(10, 21)]
    [TestCase(-5, 6)]
    public void 챔피언상태_공_변경(int amount, int expected)
    {
        var target = TestHelper.CreateStatus(11, 0, 0);
        var sut = new AttackChanger(amount);

        sut.Do(target);

        Assert.AreEqual(expected, target.Stat.Attack);
    }

    [Test]
    [TestCase(5, 15)]
    [TestCase(-3, 7)]
    public void 챔피언상태_방_변경(int amount, int expected)
    {
        var target = TestHelper.CreateStatus(0, 10, 0);
        var sut = new DefenseChanger(amount);

        sut.Do(target);

        Assert.AreEqual(expected, target.Stat.Defense);
    }

    [Test]
    [TestCase(3, 10)]
    [TestCase(-2, 5)]
    public void 챔피언상태_속_변경(int amount, int expected)
    {
        var target = TestHelper.CreateStatus(0, 0, 7);
        var sut = new SpeedChanger(amount);

        sut.Do(target);

        Assert.AreEqual(expected, target.Stat.Speed);
    }

    [Test]
    public void 챔피언_방어만_지정값으로_고정()
    {
        var target = TestHelper.CreateStatus(0, 0, 7);
        var sut = new DefenseFixer(100);

        sut.Do(target);

        Assert.AreEqual(TestHelper.CreateStat(0, 100, 7), target.Stat);
    }

    [Test]
    public void 특성_제외시키기()
    {
        var target = TestHelper.CreateStatus(0, 0, 7);
        var sut = new SkillExcluder();

        sut.Do(target);

        Assert.IsTrue(target.IsTraitExcluded);
    }
}
