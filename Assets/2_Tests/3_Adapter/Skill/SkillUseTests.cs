using NUnit.Framework;
using static TestHelper;

public class SkillUseTests
{
    [Test]
    public void 스킬_사용()
    {
        SlotStorage<Champion> champions = new();
        var datas = CreateSkills(CreateConditionFreeSkill(StatType.Attack, 10, SelfAllRule), CreateConditionFreeSkill(StatType.Attack, 20, SelfAllRule));
        champions.AddSlot(Team.Blue, CreateChampion(skillData: datas));
        var sut = new SkillUsecase(champions, CreateSkillRunner());
        SlotData callSlot = RedOneSlot;
        sut.OnUseSkill += slot => callSlot = slot;

        sut.UseSkill(BlueZeroSlot, new SlotData[] { BlueZeroSlot });

        Assert.AreEqual(30, champions.GetSlot(BlueZeroSlot).Status.Stat.Attack);
        Assert.AreEqual(BlueZeroSlot, callSlot);
    }
}
