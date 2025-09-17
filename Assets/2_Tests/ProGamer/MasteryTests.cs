using NUnit.Framework;

public class MasteryTests
{
    [Test]
    public void 숙련도_가져오기()
    {
        ChampionMastery[] data = new ChampionMastery[]{new ChampionMastery(12, 10), new ChampionMastery(11, 20)};

        ProGamer sut = new(data);

        Assert.AreEqual(10, sut.GetMastery(12));
        Assert.AreEqual(20, sut.GetMastery(11));
        Assert.AreEqual(0, sut.GetMastery(44));
    }

    [Test]
    public void 기존_챔피언은_숙련도_1_증가()
    {
        var sut = new ProGamer(new[] { new ChampionMastery(10, 5) });

        sut.AddMastery(10);

        Assert.AreEqual(6, sut.GetMastery(10));
    }

    [Test]
    public void 없는_챔피언은_숙련도_1로_추가()
    {
        var sut = new ProGamer(new[] { new ChampionMastery(10, 5) });

        sut.AddMastery(44);

        Assert.AreEqual(1, sut.GetMastery(44));
    }
}
