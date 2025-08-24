using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MatchResultCalculateTests
{
    // 편의 헬퍼
    private static ChampionStatData CreateStat(int atk = 0, int def = 0, int rng = 0, int spd = 0)
        => new ChampionStatData(atk, def, rng, spd);

    BonusCalculator CreateEmptyBonus() => new BonusCalculator(new System.Collections.Generic.SortedDictionary<int, int>());
    TeamScoreCalculator CreateScoreCalculator()
    {
        return new TeamScoreCalculator(new ChampionBonusCalculator(CreateEmptyBonus(), CreateEmptyBonus()),
            new TeamBonusCalculator(CreateEmptyBonus(), CreateEmptyBonus(), CreateEmptyBonus(), CreateEmptyBonus()));
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
        var sut = new MatchResultCalculator(CreateScoreCalculator());

        MatchResult result = sut.CalculateResult(blue, red);

        Assert.AreEqual(31, result.BlueScore);
        Assert.AreEqual(32, result.RedScore);
        Assert.AreEqual(Team.Red, result.Winner);
    }

    [Test]
    public void 동률이면_무승부()
    {
        var team = new[]
        {
            CreateStat(10, 10, 3, 5),
            CreateStat(8,  12, 2, 6),
        };
        var sut = new MatchResultCalculator(CreateScoreCalculator());

        MatchResult result = sut.CalculateResult(team, team);

        Assert.AreEqual(Team.All, result.Winner);
    }
}
