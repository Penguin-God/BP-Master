using NUnit.Framework;
using static TestHelper;

public class SkillUseTests
{
    [Test]
    public void 스킬_사용()
    {
        SlotStorage<ChampionStatus> statuses = CreateOneSlotStatus();
        var datas = CreateSkills(CreateConditionFreeSkill(StatType.Attack, 10, SelfAllRule), CreateConditionFreeSkill(StatType.Attack, 20, SelfAllRule));
        var sut = new SkillUsecase(statuses, CreateSkillRunner());
        SlotData callSlot = RedOneSlot;
        sut.OnUseSkill += slot => callSlot = slot;

        sut.UseSkill(BlueZeroSlot, new SlotData[] { BlueZeroSlot }, new Skill(datas));

        Assert.AreEqual(30, statuses.GetSlot(BlueZeroSlot).Stat.Attack);
        Assert.AreEqual(BlueZeroSlot, callSlot);
    }
}
