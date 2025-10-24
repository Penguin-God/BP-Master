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
        SlotStorage<bool> flags = new SlotStorage<bool>();
        flags.AddSlots(Team.Red, new bool[] { false, false });
        flags.AddSlots(Team.Blue, new bool[] { false, false });
        var facade = new SkillUseOrchestrator(statuses);

        var traits = CreateTraits ( CreateConditionFreeTrait(SkillType.AttackChanger, 10, new TraitTargetRule(Side.Opponent, targetRange)) );
        var traitStorage = new SlotStorage<IEnumerable<SkillData>>();
        traitStorage.AddSlot(Team.Blue, traits);
        traitStorage.AddSlot(Team.Blue, traits);
        traitStorage.AddSlot(Team.Red, traits);
        traitStorage.AddSlot(Team.Red, traits);

        var filter = new TraitSlotFilter(flags);
        var sut = new AI_TraitAgent(Team.Blue, filter, traitStorage, facade, new TargetCounter(2));

        sut.UseTrait(Team.Blue);

        Assert.AreEqual(10, statuses.GetSlot(RedZeroSlot).Stat.Attack);
        Assert.AreEqual(10, statuses.GetSlot(RedOneSlot).Stat.Attack);
    }
}
