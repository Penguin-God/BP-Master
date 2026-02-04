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
    public void 퍼센트_스탯_흡수()
    {
        var user = TestHelper.CreateStatus();
        var target = TestHelper.CreateStatus(att: 100);
        var sut = new DefenseAbsorber(user, new PercentCalculator(0.5f), StatType.Attack);

        sut.Do(target);

        Assert.AreEqual(50, target.Stat.Attack);
        Assert.AreEqual(50, user.Stat.Attack);
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
    public void 픽한_아군_스탯_변경()
    {
        var champ = new Champion(1, null, TestHelper.CreateStatus());
        var eventDispatcher = new PhaseActionEventDispatcher();

        var statChanger = new StatChanger(StatType.Attack, new ValueCalculator(100));
        var sut = new PickChampStatChanger(eventDispatcher, statChanger, Team.Blue);

        sut.Do(null);
        eventDispatcher.RaisePick(champ, Team.Blue);
        Assert.AreEqual(100, champ.Status.Stat.Attack);
        // 타겟이 아닌 팀은 반영 X
        eventDispatcher.RaisePick(champ, Team.Red);
        Assert.AreEqual(100, champ.Status.Stat.Attack);
    }

    [Test]
    public void 상대_스탯_복사()
    {
        var target = CreateStatus(10, 20, 30);
        var caster = CreateStatus();
        var sut = new Doppelganger(caster);

        sut.Do(target);

        Assert.AreEqual(caster.Stat, target.Stat);
    }

    [Test]
    public void 게임_종료_시_자신의_스탯_두배_상승()
    {
        var caster = CreateStatus(100, 100, 100);
        var dispatcher = new PhaseEventDispatcher();
        var sut = new FinalStatChanger(caster, dispatcher, new PercentCalculator(1f));

        sut.Do(null);

        Assert.AreEqual(100, caster.Stat.Attack); // 아직 종료 이벤트가 발생하지 않았으므로 스탯은 그대로여야 함
        dispatcher.Dispatch(GamePhase.Done, Team.All);
        Assert.AreEqual(200, caster.Stat.Attack);
        Assert.AreEqual(200, caster.Stat.Defense);
        Assert.AreEqual(200, caster.Stat.Speed);
    }
}
