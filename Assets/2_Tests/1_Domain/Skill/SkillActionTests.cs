using NUnit.Framework;
using static TestHelper;

public class SkillActionTests
{
    [Test]
    [TestCase(StatType.Attack, 110, 100, 100)]
    [TestCase(StatType.Defense, 100, 110, 100)]
    [TestCase(StatType.Speed, 100, 100, 110)]
    public void 챔피언_스탯_변경(StatType statType, int att, int def, int speed)
    {
        var target = CreateStatus(100, 100, 100);
        var sut = new StatChanger(statType, new PercentCalculator(0.1f));

        sut.Do(target);

        Assert.AreEqual(att, target.Stat.Attack);
        Assert.AreEqual(def, target.Stat.Defense);
        Assert.AreEqual(speed, target.Stat.Speed);
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
