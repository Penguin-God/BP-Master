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
    public void 증가분에는_UpRate를_적용한다()
    {
        var status = CreateStatus();
        status.AddUpRate(0.5f);
        status.AddDownRate(10000f); // 무시

        status.ChangeStatWithRate(CreateStat(att: 100));

        Assert.AreEqual(150, status.Stat.Attack);
    }

    [Test]
    public void 감소분에는_DownRate를_적용한다()
    {
        var status = CreateStatus(def: 100);
        status.AddUpRate(100000f); // 무시
        status.AddDownRate(downRate: -0.5f);

        status.ChangeStatWithRate(CreateStat(def: 50));

        Assert.AreEqual(75, status.Stat.Defense);
    }

    [Test]
    public void 개별_스탯_변경()
    {
        var status = CreateStatus(att:100);
        status.AddUpRate(0.5f);

        status.ChangeAttackWithRate(30);
        status.ChangeDefenseWithRate(50);

        Assert.AreEqual(30, status.Stat.Attack);
        Assert.AreEqual(75, status.Stat.Defense);
    }

    [Test]
    public void 일반_변경_함수는_Rate_무시()
    {
        var status = CreateStatus(att: 100, def:100);
        status.AddUpRate(10000f);
        status.AddDownRate(10000f);

        status.ChangeStat(CreateStat(att: 200, def: 50));

        // attack 증가분, defense 감소분 무시
        Assert.AreEqual(200, status.Stat.Attack);
        Assert.AreEqual(50, status.Stat.Defense);
    }

    [Test]
    public void 상태_깊은복사_시_값은_동일하지만_이벤트_구독자는_초기화()
    {
        var original = new ChampionStatus(CreateStat(10, 20, 30), TraitType.Charge);
        original.AddUpRate(0.5f);
        original.TraitExcluded();

        bool flag = false;
        original.OnStatChanged += (_, __) => flag = true;

        var copy = original.DeepCopy();

        Assert.AreEqual(original.Stat, copy.Stat);
        Assert.AreEqual(original.TraitType, copy.TraitType);
        Assert.AreEqual(original.IsSkillExcluded, copy.IsSkillExcluded);
        Assert.AreEqual(original.UpRate, copy.UpRate, delta: 1e-6f);
        Assert.AreEqual(original.DownRate, copy.DownRate, delta: 1e-6f);

        copy.ChangeStat(CreateStat(11, 20, 30));
        Assert.IsFalse(flag);
    }
}
