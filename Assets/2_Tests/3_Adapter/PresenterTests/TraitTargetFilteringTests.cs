using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

public class TraitTargetFilteringTests
{
    TargetCountCalculator CreateSut(int blueCount = 3, int redCount = 2) => new TargetCountCalculator(blueCount, redCount);
    [Test]
    public void Self_사이드면_자기팀_슬롯만_반환()
    {
        var sut = CreateSut(3, 2);

        var blueSlots = sut.FilteringTargetSlots(Team.Blue, new[] { Side.Self }).ToArray();
        Assert.AreEqual(3, blueSlots.Count());
    }

    [Test]
    public void Opponent_사이드면_상대팀_슬롯만_반환()
    {
        var sut = CreateSut(3, 2);

        var fromBlue = sut.FilteringTargetSlots(Team.Blue, new[] { Side.Opponent }).ToArray();

        Assert.AreEqual(2, fromBlue.Count());
    }

    [Test]
    public void Self와_Opponent가_섞이면_All로_머지되어_양팀_슬롯_모두_반환()
    {
        var sut = CreateSut(3, 2);

        var slots = sut.FilteringTargetSlots(Team.Blue, new[] { Side.Self, Side.Opponent });

        Assert.AreEqual(5, slots.Count());
        Assert.AreEqual(3, slots.Count(x => x.Team == Team.Blue));
        Assert.AreEqual(2, slots.Count(x => x.Team == Team.Red));
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

    SlotStorage<string> CreateSut(IEnumerable<string> blueItems, IEnumerable<string> redItems)
    {
        var storage = new SlotStorage<string>();
        storage.AddSlots(Team.Blue, blueItems);
        storage.AddSlots(Team.Red, redItems);
        return storage;
    }
}
