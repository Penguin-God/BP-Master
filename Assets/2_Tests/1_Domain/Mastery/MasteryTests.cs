using NUnit.Framework;

public class MasteryTests
{
    [Test]
    public void 숙련도_가져오기()
    {
        ChampionMastery[] data = new ChampionMastery[] {
            new ChampionMastery(11, 20)
        };

        MasteryCollection sut = new(data);

        Assert.AreEqual(20, GetMasteryStat(sut, 11).Attack);
        Assert.AreEqual(0, GetMasteryStat(sut, 44).Attack);

        CollectionAssert.AreEquivalent(data, sut.AllMasteries);
    }

    [Test]
    public void 기존_챔피언은_숙련도_1_증가()
    {
        var sut = new MasteryCollection(new[] { new ChampionMastery(10, 5) });

        sut.AddMastery(10);

        // 레벨이 5 -> 6으로 증가했으므로 스탯도 6이 됩니다.
        Assert.AreEqual(6, GetMasteryStat(sut, 10).Attack);
        Assert.AreEqual(6, GetMasteryStat(sut, 10).Defense);
    }

    [Test]
    public void 없는_챔피언은_숙련도_1로_추가()
    {
        var sut = new MasteryCollection(new[] { new ChampionMastery(10, 5) });

        sut.AddMastery(44);

        Assert.AreEqual(1, GetMasteryStat(sut, 44).Attack);
    }

    // 헬퍼 함수가 단일 int 레벨이 아닌 ChampionStatData 전체를 반환하도록 변경했습니다.
    ChampionStatData GetMasteryStat(MasteryCollection sut, int id) => sut.GetMasteryStat(id);
}