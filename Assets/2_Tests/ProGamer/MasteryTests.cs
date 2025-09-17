using NUnit.Framework;

public class MasteryTests
{
    [Test]
    public void 숙련도_가져오기()
    {
        ChampionMastery[] data = new ChampionMastery[]{new ChampionMastery(12, 10), new ChampionMastery(11, 20)};

        ProGamer sut = new();

        Assert.AreEqual(10, sut.GetMastery(12));
        Assert.AreEqual(20, sut.GetMastery(11));
        Assert.AreEqual(0, sut.GetMastery(44));
    }
}
