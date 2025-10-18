using NUnit.Framework;
using System.Collections.Generic;
using static TestHelper;

public class AI_TraitUseTests
{
    [Test]
    public void AI_특성_사용()
    {
        var statuses = CreateOneSlotStatus();
        SlotStorage<TraitApplier> applilers = CreateOneSlotApplier(statuses);
        var facade = new TraitUseFacade(applilers);

        var traitStorage = new SlotStorage<IEnumerable<TraitData>>();
        var traits = new TraitData[] { TestHelper.CreateTraitData(TraitType.AttackChanger, 10, side: Side.Opponent, range: TargetRange.Single) };
        traitStorage.AddSlot(Team.Blue, traits);
        traitStorage.AddSlot(Team.Red, traits);


        var filter = new TraitSlotFilter(applilers);
        var sut = new AI_TraitAgent(Team.Blue, filter, traitStorage, facade);

        sut.UseTrait(Team.Blue);

        Assert.IsTrue(applilers.GetSlot(BlueZeroSlot).IsUse);
        Assert.AreEqual(10, statuses.GetSlot(RedZeroSlot).Stat.Attack);
    }
}
