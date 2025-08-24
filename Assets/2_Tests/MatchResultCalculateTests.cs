using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MatchResultCalculateTests
{
    // 편의 헬퍼
    private static ChampionStatData CreateStat(int atk, int def, int rng, int spd)
        => new ChampionStatData(atk, def, rng, spd);

    [Test]
    public void 다수_챔피언_스쿼드_결과()
    {
        // Arrange
        var blue = new[]
        {
            CreateStat(12, 4,  2, 5),
            CreateStat(7,  9,  1, 3),
            CreateStat(5,  5,  5, 5),
        };
        var red = new[]
        {
            CreateStat(10, 6,  3, 4),
            CreateStat(6,  10, 2, 2),
            CreateStat(8,  3,  4, 6),
        };

        // Act
        MatchResult result = new MatchResultCalculator().CalculateResult(blue, red);

        // Assert (여기도 네가 기대값만 채우면 됨)
        var expectedBlue = /* TODO */ 0;
        var expectedRed = /* TODO */ 0;
        var expectedWinner = /* TODO */ Team.Red; // 또는 Team.Blue / 무승부 값

        Assert.AreEqual(expectedBlue, result.BlueScore);
        Assert.AreEqual(expectedRed, result.RedScore);
        Assert.AreEqual(expectedWinner, result.Winner);
    }

    [Test]
    public void 동률이면_무승부()
    {
        var team = new[]
        {
            CreateStat(10, 10, 3, 5),
            CreateStat(8,  12, 2, 6),
        };

        // Act
        MatchResult result = new MatchResultCalculator().CalculateResult(team, team);

        // Assert
        var expectedBlue = /* TODO */ 0;
        var expectedRed = /* TODO */ 0;
        
        Assert.AreEqual(expectedBlue, result.BlueScore);
        Assert.AreEqual(expectedRed, result.RedScore);
        Assert.AreEqual(Team.All, result.Winner);
    }
}
