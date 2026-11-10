using NUnit.Framework;
using static TestHelper;

[TestFixture]
public class ChampionStatusTests
{
    [Test]
    public void 생성시_스탯이_정상적으로_저장된다()
    {
        var sut = CreateStatus(att: 10, def: 5);

        Assert.AreEqual(10, sut.Stat.Attack);
        Assert.AreEqual(5, sut.Stat.Defense);
    }

    [Test]
    public void ChangeStat_호출시_스탯이_변경_후_이벤트()
    {
        var sut = CreateStatus(1, 1);
        ChampionStatData before = default;
        ChampionStatData after = default;
        sut.OnStatChanged += (be, af) => (before, after) = (be, af);
        var newStat = CreateStat(5, 6);

        sut.ChangeStat(newStat);

        Assert.AreEqual(newStat, sut.Stat);
        Assert.AreEqual(CreateStat(1, 1), before);
        Assert.AreEqual(newStat, after);
    }

    [Test]
    public void 개별_스탯_변경()
    {
        var sut = CreateStatus(att: 100, def: 10);

        sut.AddAttack(-50);
        sut.AddDefense(60);

        Assert.AreEqual(50, sut.Stat.Attack);
        Assert.AreEqual(70, sut.Stat.Defense);
    }

    [Test]
    public void 상태_깊은복사_시_값은_동일하지만_이벤트_구독자는_초기화()
    {
        var original = new ChampionStatus(CreateStat(10, 20));

        bool flag = false;
        original.OnStatChanged += (_, __) => flag = true;

        var copy = original.DeepCopy();

        Assert.AreEqual(original.Stat, copy.Stat);

        copy.ChangeStat(CreateStat(11, 20));
        Assert.IsFalse(flag);
    }
}
