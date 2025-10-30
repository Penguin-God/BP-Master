using NUnit.Framework;

public class MasteryApplyTests
{
    [Test]
    public void 챔프_숙련도_계산()
    {
        var champ1 = new ChampionStatData(10, 5, 3);
        MasteryCalculator sut = new();

        var result = sut.ApplyMastery(champ1, 10);

        Assert.AreEqual(20, result.Attack);
        Assert.AreEqual(15, result.Defense);
        Assert.AreEqual(3, result.Speed);
    }

    [Test]
    public void 숙련도에_따라_계산()
    {
        var champ1 = new ChampionStatData(10, 5, 3);
        MasteryCalculator sut = new();

        var result = sut.ApplyMastery(champ1, 10);

        Assert.AreEqual(20, result.Attack);
        Assert.AreEqual(15, result.Defense);
        Assert.AreEqual(3, result.Speed);
    }
}
