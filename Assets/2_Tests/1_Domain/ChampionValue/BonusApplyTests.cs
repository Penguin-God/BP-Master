using NUnit.Framework;
using System.Collections.Generic;

public class BonusApplyTests
{
    [Test]
    public void 보너스_점수에_적용()
    {
        TeamBonusCalculator teamBonusCalculator = new TeamBonusCalculator(Bonus(300, 100), Bonus(300, 100), Bonus(30, 100));

        // GameScoreInfo result = teamBonusCalculator.ApplyBonus();

        // Assert.AreEqual(result.Blue, result.Red);
    }

    BonusCalculator Bonus(int needScore, int bonus) => new(new SortedDictionary<int, int>() { { needScore, bonus } });
}
