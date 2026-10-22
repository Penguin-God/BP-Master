using NUnit.Framework;
using static TestHelper;

public class SkillActionTests
{
    [Test]
    [TestCase(StatType.Attack)]
    [TestCase(StatType.Defense)]
    [TestCase(StatType.Speed)]
    public void 챔피언_스탯_변경(StatType statType)
    {
        var target = CreateStatus(100, 100, 100);
        var sut = new StatChanger(statType, () => 100, () => target);

        sut.Do(target);

        Assert.AreEqual(200, target.Stat.GetStatValue(statType));
    }

    //[Test]
    //public void 픽한_아군_스탯_변경()
    //{
    //    var champ = new Champion(1, null, TestHelper.CreateStatus());
    //    var eventDispatcher = BanPickEventDispatcher.Create();

    //    var statChanger = new StatChanger(StatType.Attack, new ValueCalculator(100));
    //    var sut = new PickChampStatChanger(eventDispatcher, statChanger, Team.Blue);

    //    sut.Do(null);
    //    eventDispatcher.RaisePick(champ, BlueZeroSlot);
    //    Assert.AreEqual(100, champ.Status.Stat.Attack);
    //    // 타겟이 아닌 팀은 반영 X
    //    eventDispatcher.RaisePick(champ, RedZeroSlot);
    //    Assert.AreEqual(100, champ.Status.Stat.Attack);
    //}

    [Test]
    public void 게임_종료_시_자신의_스탯_두배_상승()
    {
        var caster = CreateStatus(100, 100, 100);
        var dispatcher = PhaseEventDispatcher.Create();
        var sut = new FinalStatChanger(caster, dispatcher, new PercentCalculator(1f));

        sut.Do(null);

        Assert.AreEqual(100, caster.Stat.Attack); // 아직 종료 이벤트가 발생하지 않았으므로 스탯은 그대로여야 함
        dispatcher.Dispatch(GamePhase.Done, Team.All);
        Assert.AreEqual(200, caster.Stat.Attack);
        Assert.AreEqual(200, caster.Stat.Defense);
        Assert.AreEqual(200, caster.Stat.Speed);
    }
}
