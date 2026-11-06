using System.Collections.Generic;
using NUnit.Framework;

public class BattleResultCalculatorTests
{
    Dictionary<Team, int> CreatePlayerIds(int blueId = 10, int redId = 20) => new Dictionary<Team, int>
    {
        { Team.Blue, blueId },
        { Team.Red, redId }
    };

    [TestCase(Team.Blue, 10)]
    [TestCase(Team.Red, 20)]
    [TestCase(Team.All, 20)]
    public void 매치_결과에_따라_승리한_측의_플레이어_ID가_반환된다(Team winner, int expectedId)
    {
        var playerIds = CreatePlayerIds(blueId: 10, redId: 20);

        var winnerId = BattleResultLogic.GetWinnerId(playerIds, winner);

        Assert.AreEqual(expectedId, winnerId);
    }
}