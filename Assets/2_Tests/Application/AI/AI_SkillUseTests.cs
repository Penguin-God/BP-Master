using NUnit.Framework;
using static TestHelper;

public class AI_SkillUseTests
{
    [Test]
    public void UseSkill_호출하면_SkillUseController_OnUseSkill_이벤트가_발생한다()
    {
        var statuses = CreateTwoSlotStatus();
        var skillUseController = new SkillUseController(statuses, CreateSkillExceutorFactory());

        var skill = CreateValueSkill(SkillType.AttackChanger, 10, rule: SelfTriple);
        var skillSlots = new SlotStorage<Skill>();
        skillSlots.AddSlot(Team.Blue, skill);
        skillSlots.AddSlot(Team.Blue, skill);
        skillSlots.AddSlot(Team.Red, null);
        skillSlots.AddSlot(Team.Red, null);

        var sut = new AI_SkillUseAgent(skillSlots, skillUseController);

        sut.UseSkill(BlueOneSlot);

        Assert.AreEqual(10, statuses.GetSlot(BlueZeroSlot).Stat.Attack);
        Assert.AreEqual(10, statuses.GetSlot(BlueOneSlot).Stat.Attack);
        Assert.AreEqual(0, statuses.GetSlot(RedZeroSlot).Stat.Attack);
        Assert.AreEqual(0, statuses.GetSlot(RedOneSlot).Stat.Attack);
    }
}
