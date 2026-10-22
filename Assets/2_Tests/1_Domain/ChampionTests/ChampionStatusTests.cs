using NUnit.Framework;
using static TestHelper;

[TestFixture]
public class ChampionStatusTests
{
    [Test]
    public void 생성시_스탯이_정상적으로_저장된다()
    {
        var status = CreateStatus(att: 10, def: 5, speed: 3);

        Assert.AreEqual(10, status.Stat.Attack);
        Assert.AreEqual(5, status.Stat.Defense);
        Assert.AreEqual(3, status.Stat.Speed);
    }

    [Test]
    public void ChangeStat_호출시_스탯이_변경_후_이벤트()
    {
        var status = CreateStatus(1, 1, 1);
        ChampionStatData before = default;
        ChampionStatData after = default;
        status.OnStatChanged += (be, af) => (before, after) = (be, af);
        var newStat = CreateStat(5, 6, 7);

        status.ChangeStat(newStat);

        Assert.AreEqual(newStat, status.Stat);
        Assert.AreEqual(CreateStat(1, 1, 1), before);
        Assert.AreEqual(newStat, after);
    }

    [Test]
    public void 개별_스탯_변경()
    {
        var status = CreateStatus(att:100, def:10);

        status.AddAttack(-50);
        status.AddDefense(60);
        status.AddSpeed(1);

        Assert.AreEqual(50, status.Stat.Attack);
        Assert.AreEqual(70, status.Stat.Defense);
        Assert.AreEqual(1, status.Stat.Speed);
    }

    [Test]
    public void 상태_깊은복사_시_값은_동일하지만_이벤트_구독자는_초기화()
    {
        var original = new ChampionStatus(CreateStat(10, 20, 30));

        bool flag = false;
        original.OnStatChanged += (_, __) => flag = true;

        var copy = original.DeepCopy();

        Assert.AreEqual(original.Stat, copy.Stat);

        copy.ChangeStat(CreateStat(11, 20, 30));
        Assert.IsFalse(flag);
    }
}
