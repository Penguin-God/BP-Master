using NUnit.Framework;

public class MatchResultCalculateTests
{
    ChampionStatData CreateStat(int atk = 0, int def = 0, int spd = 0) => new ChampionStatData(atk, def, spd);

    BonusCalculator CreateEmptyBonus() => new BonusCalculator(new System.Collections.Generic.SortedDictionary<int, int>());
    
    [Test]
    public void 스코어_합_계산()
    {
        TeamScoreInfo sut = new TeamScoreInfo(10, 10, 2, 2, 2);

        Assert.AreEqual(20, sut.DefaultScore);
        Assert.AreEqual(6, sut.BonusScore);
        Assert.AreEqual(26, sut.Total);
    }

    [Test]
    public void 다수_챔피언_스쿼드_결과()
    {
        var blue = new[]
        {
            CreateStat(12, 4),
            CreateStat(7,  8),
        };
        var red = new[]
        {
            CreateStat(10, 6),
            CreateStat(10, 6),
        };
        var sut = new MatchResultCalculator(new TeamBonusCalculator(CreateEmptyBonus(), CreateEmptyBonus(), CreateEmptyBonus()));

        MatchResult result = sut.CalculateResult(blue, red);

        // 변경 포인트: BlueScore/RedScore → BlueInfo.Total / RedInfo.Total
        Assert.AreEqual(31, result.BlueInfo.Total);
        Assert.AreEqual(32, result.RedInfo.Total);
        Assert.AreEqual(Team.Red, result.Winner);
    }

    [Test]
    public void 동률이면_무승부()
    {
        var team = new[]
        {
            CreateStat(10, 10, 5),
            CreateStat(8,  12, 6),
        };
        var sut = new MatchResultCalculator(new TeamBonusCalculator(CreateEmptyBonus(), CreateEmptyBonus(), CreateEmptyBonus()));

        MatchResult result = sut.CalculateResult(team, team);

        Assert.AreEqual(Team.All, result.Winner);
    }
}