using NUnit.Framework;

public class ChampionStatusTextBuildTests
{
    [Test]
    public void 챔피언_스탯정보가_텍스트로_생성()
    {
        ChampionStatModel result = ChampionStatusTextBuilder.CreateStatViewModel(new ChampionStatData(10, 12, 6));

        Assert.AreEqual("공 10", result.Attack);
        Assert.AreEqual("방 12", result.Defense);
        Assert.AreEqual("속도 6", result.Speed);
    }
}
