using NUnit.Framework;
using System.Collections.Generic;

public class AI_TraitUseTests
{
    [Test]
    public void AI_특성_사용()
    {
        var statuses = new SlotStorage<ChampionStatus>();
        statuses.AddSlot(Team.Blue, new ChampionStatus(default));
        statuses.AddSlot(Team.Red, new ChampionStatus(default));

        var traitStorage = new SlotStorage<IEnumerable<TraitData>>();
        var traits = new TraitData[] { TestHelper.CreateTraitData(TraitType.AttackChanger, 10, side: Side.Opponent, range: TargetRange.Single) };

        traitStorage.AddSlot(Team.Blue, traits);
        traitStorage.AddSlot(Team.Red, traits);

        
        SlotStorage<TraitApplier> applilers = new();
        applilers.AddSlot(Team.Blue, new TraitApplier(statuses));
        applilers.AddSlot(Team.Red, new TraitApplier(statuses));

        var facade = new TraitUseFacade(applilers);
        var filter = new TraitSlotFilter(applilers);
        var sut = new AI_TraitAgent(Team.Blue, filter, traitStorage, facade);

        sut.UseTrait(Team.Blue);

        Assert.IsTrue(applilers.GetSlot(TestHelper.CreateBlueSlot(0)).IsUse);
        Assert.AreEqual(10, statuses.GetSlot(TestHelper.CreateRedSlot(0)).Stat.Attack);
    }
}
