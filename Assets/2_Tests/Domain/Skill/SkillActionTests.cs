using NUnit.Framework;
using static TestHelper;

public class TraitActionTests
{
    [Test]
    public void 챔피언_공_변경()
    {
        var target = TestHelper.CreateStatus(11, 0, 0);
        var sut = new AttackChanger(new FixCalculator(100));

        sut.Do(target);

        Assert.AreEqual(100, target.Stat.Attack);
    }

    [Test]
    public void 챔피언_방_변경()
    {
        var target = TestHelper.CreateStatus(0, 10, 0);
        var sut = new DefenseChanger(new ValueCalculator(100));

        sut.Do(target);

        Assert.AreEqual(110, target.Stat.Defense);
    }

    [Test]
    [TestCase(3, 10)]
    [TestCase(-2, 5)]
    public void 챔피언상태_속_변경(int amount, int expected)
    {
        var target = TestHelper.CreateStatus(0, 0, 7);
        var sut = new SpeedChanger(new ValueCalculator(amount));

        sut.Do(target);

        Assert.AreEqual(expected, target.Stat.Speed);
    }

    [Test]
    public void 특성_제외시키기()
    {
        var target = TestHelper.CreateStatus(0, 0, 7);
        var sut = new SkillExcluder();

        sut.Do(target);

        Assert.IsTrue(target.IsSkillExcluded);
    }

    [Test]
    public void 퍼센트_방어_흡수()
    {
        var user = TestHelper.CreateStatus();
        var target = TestHelper.CreateStatus(0, def: 100);
        var sut = new DefenseAbsorber(user, new PercentCalculator(0.5f));

        sut.Do(target);

        Assert.AreEqual(50, target.Stat.Defense);
        Assert.AreEqual(50, user.Stat.Defense);
    }

    [Test]
    public void 자신의_스탯_비율만큼_증가()
    {
        var user = TestHelper.CreateStatus(100, 100);
        var target = TestHelper.CreateStatus();
        var sut = new Resonance(user, 0.5f);

        sut.Do(target);

        Assert.AreEqual(50, target.Stat.Attack);
        Assert.AreEqual(50, target.Stat.Defense);
    }

    [Test]
    public void 증폭율_변경()
    {
        var target = TestHelper.CreateStatus();
        var sut = new AmplifyChanger(0.5f);

        sut.Do(target);

        Assert.AreEqual(1.5f, target.UpRate);
    }

    [Test]
    public void 픽한_아군_스탯_증가()
    {
        var champ = new Champion(1, null, CreateStatus());
        var eventDispatcher = new PhaseActionEventDispatcher();
        var sut = new PickChampBuffer(eventDispatcher, 100);

        sut.Do(null);
        eventDispatcher.RaisePick(champ);


        Assert.AreEqual(100, champ.Status.Stat.Attack);
    }
}
