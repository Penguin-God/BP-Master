using NUnit.Framework;

public class MasteryTests
{
    [Test]
    public void 숙련도_가져오기()
    {
        ChampionMastery[] data = new ChampionMastery[]{new ChampionMastery(12, 10), new ChampionMastery(11, 20)};

        MasteryCollection sut = new(data);

        Assert.AreEqual(10, GetMastery(sut, 12));
        Assert.AreEqual(20, GetMastery(sut, 11));
        Assert.AreEqual(0, GetMastery(sut, 44));
        CollectionAssert.AreEquivalent(data, sut.AllMasteries);
    }

    [Test]
    public void 기존_챔피언은_숙련도_1_증가()
    {
        var sut = new MasteryCollection(new[] { new ChampionMastery(10, 5) });

        sut.AddMastery(10);

        Assert.AreEqual(6, GetMastery(sut, 10));
    }

    [Test]
    public void 없는_챔피언은_숙련도_1로_추가()
    {
        var sut = new MasteryCollection(new[] { new ChampionMastery(10, 5) });

        sut.AddMastery(44);

        Assert.AreEqual(1, GetMastery(sut, 44));
    }

    int GetMastery(MasteryCollection sut, int id) => sut.GetMasteryLevel(id);
}
