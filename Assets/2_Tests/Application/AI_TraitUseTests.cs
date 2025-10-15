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

        var facade = new TraitUseFacade(statuses);
        var filter = new TraitSlotFilter(teamSize: 1, facade);

        var sut = new AI_TraitAgent(Team.Blue, filter, traitStorage, facade);

        sut.UseTrait(Team.Blue);

        Assert.IsTrue(statuses.GetSlot(TestHelper.CreateBlueSlot(0)).IsUseTrait);
        Assert.AreEqual(10, statuses.GetSlot(TestHelper.CreateRedSlot(0)).Stat.Attack);
    }
}
