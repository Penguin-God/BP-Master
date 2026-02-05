using NUnit.Framework;
using static TestHelper;

public class SkillUseTests
{
    [Test]
    public void 스킬_사용()
    {
        SlotStorage<PickChampion> pickChampions = new();
        var datas = CreateSkills(CreateConditionFreeSkill(StatType.Attack, 10, SelfAllRule), CreateConditionFreeSkill(StatType.Attack, 20, SelfAllRule));
        pickChampions.AddSlot(Team.Blue, new PickChampion(0, new Skill(datas), CreateStatus(), Team.Blue));
        var sut = new SkillUsecase(pickChampions, CreateSkillRunner());
        SlotData callSlot = RedOneSlot;
        sut.OnUseSkill += slot => callSlot = slot;

        sut.UseSkill(BlueZeroSlot, new SlotData[] { BlueZeroSlot });

        Assert.AreEqual(30, pickChampions.GetSlot(BlueZeroSlot).Status.Stat.Attack);
        Assert.AreEqual(BlueZeroSlot, callSlot);
    }
}
