using NUnit.Framework;
using System.Collections.Generic;

public class TraitTests
{
    [Test]
    public void 돌격_보유자만_특성_수에_곱한_값만큼_공격_증가()
    {
        const int AttIncreseAmount = 5;
        var statuses = new ChampionStatus[] { CreateStatus(TraitType.Charge), CreateStatus(TraitType.None), CreateStatus(TraitType.Charge) };
        var sut = new Charge(AttIncreseAmount, statuses);

        sut.Do();

        Assert.AreEqual(10, statuses[0].Stat.Attack);
        Assert.AreEqual(0, statuses[1].Stat.Attack);
        Assert.AreEqual(10, statuses[2].Stat.Attack);
    }

    [Test]
    public void 가드는_넘긴_값만큼_감소율을_뺀다()
    {
        var statuses = new List<ChampionStatus> { TestHelper.CreateStatus(), TestHelper.CreateStatus() };

        var sut = new Guard(0.2f, statuses);

        sut.Do();

        Assert.AreEqual(0.8f, statuses[0].DownRate);
        Assert.AreEqual(0.8f, statuses[1].DownRate);
    }

    [Test]
    public void 증폭은_넘긴_값만큼_증가율을_더한다()
    {
        var statuses = new List<ChampionStatus> { TestHelper.CreateStatus(), TestHelper.CreateStatus() };

        var sut = new Amplifier(0.2f, statuses);

        sut.Do();

        Assert.AreEqual(1.2f, statuses[0].UpRate);
        Assert.AreEqual(1.2f, statuses[1].UpRate);
    }

    [Test]
    public void 파괴는_넘긴_값만큼_감소율을_더한다()
    {
        var statuses = new List<ChampionStatus> { TestHelper.CreateStatus(), TestHelper.CreateStatus() };

        var sut = new Break(0.2f, statuses);

        sut.Do();

        Assert.AreEqual(1.2f, statuses[0].DownRate);
        Assert.AreEqual(1.2f, statuses[1].DownRate);
    }

    ChampionStatus CreateStatus(TraitType type) => TestHelper.CreateStatus(traitType: type);
}
