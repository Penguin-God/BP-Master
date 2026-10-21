using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

public class TraitTargetFilteringTests
{
    TeamCount CreateSut(int blueCount = 3, int redCount = 2) => new TeamCount(blueCount, redCount);
    SlotStorage<string> CreateSut(IEnumerable<string> blueItems, IEnumerable<string> redItems)
    {
        var storage = new SlotStorage<string>();
        storage.AddSlots(Team.Blue, blueItems);
        storage.AddSlots(Team.Red, redItems);
        return storage;
    }


    [Test]
    public void Self_사이드면_자기팀_슬롯만_반환()
    {
        var sut = CreateSut(3, 2);

        var blueSlots = TargetCountCalculator.CreateTargetingableSlots(Team.Blue, Side.Self , sut).ToArray();

        Assert.AreEqual(3, blueSlots.Length);
    }

    [Test]
    public void Opponent_사이드면_상대팀_슬롯만_반환()
    {
        var sut = CreateSut(3, 2);

        var fromBlue = TargetCountCalculator.CreateTargetingableSlots(Team.Blue, Side.Opponent , sut).ToArray();

        Assert.AreEqual(2, fromBlue.Length);
    }

    [Test]
    public void 아군_지정_시_시전자_팀의_슬롯_아이템만_반환된다()
    {
        var sut = CreateSut(new[] { "Blue1", "Blue2" }, new[] { "Red1" });

        var result = TargetCountCalculator.FilterTargets(sut, Team.Blue, new[] { Side.Self }).ToList();

        CollectionAssert.AreEqual(new[] { "Blue1", "Blue2" }, result);
    }

    [Test]
    public void 적군_지정_시_상대_팀의_슬롯_아이템만_반환된다()
    {
        var sut = CreateSut(new[] { "Blue1" }, new[] { "Red1", "Red2" });

        var result = TargetCountCalculator.FilterTargets(sut, Team.Blue, new[] { Side.Opponent }).ToList();

        CollectionAssert.AreEqual(new[] { "Red1", "Red2" }, result);
    }

    [Test]
    public void 전체_지정_시_양팀의_모든_슬롯_아이템이_순서대로_반환된다()
    {
        var sut = CreateSut(new[] { "Blue1" }, new[] { "Red1" });

        var result = TargetCountCalculator.FilterTargets(sut, Team.Blue, new[] { Side.All }).ToList();

        CollectionAssert.AreEqual(new[] { "Blue1", "Red1" }, result);
    }
}