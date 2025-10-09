using NUnit.Framework;
using System;
using System.Collections.Generic;

public class AI_TraitUseTests
{
    [Test]
    public void AI_특성_사용()
    {
        var statuses = new SlotStorage<ChampionStatus>();
        statuses.AddSlot(Team.Blue, new ChampionStatus(new ChampionStatData(0, 0, 0)));
        statuses.AddSlot(Team.Red, new ChampionStatus(new ChampionStatData(0, 0, 0)));

        var traitStorage = new SlotStorage<IEnumerable<TraitData>>();
        var rule = new TraitTargetRule(Side.Self, TargetRange.Single);
        var traits = new TraitData[] { new TraitData(TraitType.AttackChanger, 10, TraitConditionType.None, 0, rule) };

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
