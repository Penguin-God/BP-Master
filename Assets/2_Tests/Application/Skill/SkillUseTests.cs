using NUnit.Framework;
using static TestHelper;

public class SkillUseTests
{
    [Test]
    public void 스킬_사용()
    {
        SlotStorage<ChampionStatus> statuses = CreateOneSlotStatus();
        var datas = CreateSkills(CreateConditionFreeSkill(SkillType.AttackChanger, 10, SelfAllRule), CreateConditionFreeSkill(SkillType.DefenseChanger, 10, SelfAllRule));
        var sut = new SkillUseController(statuses);
        SlotData callSlot = RedOneSlot;
        sut.OnUseSkill += slot => callSlot = slot;

        sut.UseSkill(BlueZeroSlot, new SlotData[] { BlueZeroSlot }, new Skill(datas));

        Assert.AreEqual(10, statuses.GetSlot(BlueZeroSlot).Stat.Attack);
        Assert.AreEqual(10, statuses.GetSlot(BlueZeroSlot).Stat.Defense);
        Assert.AreEqual(BlueZeroSlot, callSlot);
    }
}
