using NUnit.Framework;

public class MasteryApplyTests
{
    [Test]
    public void 챔프_숙련도_적용()
    {
        var champ1 = TestHelper.CreateStatus(10, 5, 0);
        var champ2 = TestHelper.CreateStatus(10, 5, 0);
        MasteryApplier sut = new();

        // 숙련도 적용
        sut.ApplyMastery(champ1, 10);
        Assert.AreEqual(20, champ1.StatData.Attack);
        Assert.AreEqual(15, champ1.StatData.Defense);

        // 숙련도 없어서 스탯 그대로
        sut.ApplyMastery(champ2, 0);
        sut.ApplyMastery(champ2, -33);
        Assert.AreEqual(10, champ2.StatData.Attack);
        Assert.AreEqual(5, champ2.StatData.Defense);
    }

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
}
