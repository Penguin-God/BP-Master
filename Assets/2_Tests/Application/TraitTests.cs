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
    public void Guard가_모든_Status에_적용된다()
    {
        var statuses = new List<ChampionStatus> { TestHelper.CreateStatus(), TestHelper.CreateStatus() };

        var sut = new Guard(0.2f, statuses);

        sut.Do();

        Assert.AreEqual(0.8f, statuses[0].DownRate);
        Assert.AreEqual(0.8f, statuses[0].DownRate);
    }

    ChampionStatus CreateStatus(TraitType type) => TestHelper.CreateStatus(traitType: type);
}
