using NUnit.Framework;
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
        var facade = new SkillUseController(statuses);

        var skill = CreateSkill(SkillType.AttackChanger, 10, traitTargetRule: new TraitTargetRule(Side.Opponent, targetRange));
        var skillStorage = new SlotStorage<Skill>();
        skillStorage.AddSlot(Team.Blue, skill);
        skillStorage.AddSlot(Team.Blue, skill);
        skillStorage.AddSlot(Team.Red, null);
        skillStorage.AddSlot(Team.Red, null);

        var filter = new SkillSlotFilter(flags);
        var sut = new AI_TraitAgent(Team.Blue, filter, skillStorage, facade, new TargetCounter(2));

        sut.UseTrait(Team.Blue);

        Assert.AreEqual(10, statuses.GetSlot(RedZeroSlot).Stat.Attack);
        Assert.AreEqual(10, statuses.GetSlot(RedOneSlot).Stat.Attack);
    }
}
