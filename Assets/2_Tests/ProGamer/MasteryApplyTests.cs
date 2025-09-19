using NUnit.Framework;

public class MasteryApplyTests
{
    [Test]
    public void 챔프_숙련도_적용()
    {
        var gamer = CreateGamer(1, 10);
        var champ1 = new Champion(1, "", new ChampionStatData(10, 5, 0), default, null);
        var champ2 = new Champion(2, "", new ChampionStatData(10, 5, 0), default, null);
        MasteryApplier sut = new();

        // 숙련도 적용
        Assert.IsTrue(sut.ApplyMastery(gamer, champ1));
        Assert.AreEqual(20, champ1.StatData.Attack);
        Assert.AreEqual(15, champ1.StatData.Defense);

        // 숙련도 없어서 스탯 그대로
        Assert.IsFalse(sut.ApplyMastery(gamer, champ2));
        Assert.AreEqual(10, champ2.StatData.Attack);
        Assert.AreEqual(5, champ2.StatData.Defense);
    }

    ProGamer CreateGamer(int id, int level) => new ProGamer(new ChampionMastery[] { new ChampionMastery(id, level) });
}
