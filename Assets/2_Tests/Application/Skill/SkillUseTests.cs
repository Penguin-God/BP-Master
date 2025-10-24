using NUnit.Framework;
using static TestHelper;

public class SkillUseTests
{
    [Test]
    public void 스킬_사용()
    {
        SlotStorage<ChampionStatus> statuses = CreateOneSlotStatus();
        SkillData[] datas = CreateTraits(CreateConditionFreeTrait(SkillType.AttackChanger, 10, SelfAllRule), CreateConditionFreeTrait(SkillType.DefenseChanger, 10, SelfAllRule));
        var sut = new SkillUseOrchestrator(statuses);
        SlotData callSlot = RedOneSlot;
        sut.OnUseSkill += slot => callSlot = slot;

        sut.UseSkill(BlueZeroSlot, new SlotData[] { BlueZeroSlot }, datas);

        Assert.AreEqual(10, statuses.GetSlot(BlueZeroSlot).Stat.Attack);
        Assert.AreEqual(10, statuses.GetSlot(BlueZeroSlot).Stat.Defense);
        Assert.AreEqual(BlueZeroSlot, callSlot);
    }
}
