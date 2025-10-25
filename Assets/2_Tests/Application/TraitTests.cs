using NUnit.Framework;

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

    ChampionStatus CreateStatus(TraitType type) => TestHelper.CreateStatus(traitType: type);
}
