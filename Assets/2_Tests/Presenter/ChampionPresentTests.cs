using NUnit.Framework;

public class ChampionPresentTests
{
    [Test]
    public void 챔피언_기본정보가_텍스트로_출력된다()
    {
        var sut = new ChampionPersenter();

        ChampionViewModel result = sut.PresentStat(new ChampionStatData(10, 12, 6));

        StringAssert.Contains("공격력 : 10", result.Attack);
        StringAssert.Contains("방어력 : 12", result.Defense);
        StringAssert.Contains("속도 : 6", result.Speed);
    }
}
