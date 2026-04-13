using NUnit.Framework;

public class ChampionshipAdvencerTests
{
    [Test]
    public void 생성_시_현재_대회_타입_설정()
    {
        var sut = new ChampionshipAdvencer();

        Assert.AreEqual(MatchType.Tournament, sut.CurrentMatchType);
    }
}
