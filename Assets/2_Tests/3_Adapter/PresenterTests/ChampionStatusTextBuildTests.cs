using NUnit.Framework;

public class ChampionStatusTextBuildTests
{
    readonly ChampionStatusTextBuilder sut = new();

    [Test]
    public void 챔피언_스탯정보가_텍스트로_생성()
    {
        ChampionStatModel result = sut.CreateStatViewModel(new ChampionStatData(10, 12, 6));

        Assert.AreEqual("공 10", result.Attack);
        Assert.AreEqual("방 12", result.Defense);
        Assert.AreEqual("속도 6", result.Speed);
    }

    public void 전투_보정치_텍스트_빌드()
    {
        CombatModifierTextModel result = sut.BuildCombatModel(1.699999f, 1.3f);

        Assert.AreEqual(result.IncreaseRateText, "증가율 : 1.7");
        Assert.AreEqual(result.DecreaseRateText, "감소율 : 1.3");
    }
}
