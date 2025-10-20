using NUnit.Framework;
using System.Collections.Generic;
using static TestHelper;

public class AI_TraitUseTests
{
    [Test]
    [TestCase(TargetRange.All)]
    [TestCase(TargetRange.Double)]
    public void AI_특성_사용(TargetRange targetRange)
    {
        var statuses = CreateTwoSlotStatus();
        SlotStorage<TraitApplier> applilers = CreateTwoSlotApplier(statuses);
        var facade = new TraitUseFacade(applilers, statuses);

        var traits = CreateTraits ( CreateConditionFreeTrait(TraitType.AttackChanger, 10, new TraitTargetRule(Side.Opponent, targetRange)) );
        var traitStorage = new SlotStorage<IEnumerable<TraitData>>();
        traitStorage.AddSlot(Team.Blue, traits);
        traitStorage.AddSlot(Team.Blue, traits);
        traitStorage.AddSlot(Team.Red, traits);
        traitStorage.AddSlot(Team.Red, traits);

        var filter = new TraitSlotFilter(applilers);
        var sut = new AI_TraitAgent(Team.Blue, filter, traitStorage, facade, new TargetCounter(2));

        sut.UseTrait(Team.Blue);

        Assert.IsTrue(applilers.GetSlot(BlueZeroSlot).IsUse);
        Assert.AreEqual(10, statuses.GetSlot(RedZeroSlot).Stat.Attack);
        Assert.AreEqual(10, statuses.GetSlot(RedOneSlot).Stat.Attack);
    }
}
