using NUnit.Framework;

public class MasteryTests
{
    [Test]
    public void 숙련도_가져오기()
    {
        ChampionMastery[] data = new ChampionMastery[] {
            new ChampionMastery(11, 20)
        };

        MasteryStatCollection sut = new(data);

        Assert.AreEqual(20, GetMasteryStat(sut, 11).Attack);
        Assert.AreEqual(0, GetMasteryStat(sut, 44).Attack);

        CollectionAssert.AreEquivalent(data, sut.AllMasteries);
    }

    ChampionStatData GetMasteryStat(MasteryStatCollection sut, int id) => sut.GetMasteryStat(id);
}