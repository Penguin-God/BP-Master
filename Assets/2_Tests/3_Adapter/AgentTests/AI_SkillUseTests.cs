using NUnit.Framework;
using static TestHelper;

public class AI_SkillUseTests
{
    [Test]
    public void UseSkill_호출하면_SkillUseController_OnUseSkill_이벤트가_발생한다()
    {
        var pickChampions = new SlotStorage<PickChampion>();
        var skill = CreateValueSkill(StatType.Attack, 10, rule: SelfTriple);
        pickChampions.AddSlot(Team.Blue, new PickChampion(1, skill, CreateStatus(), Team.Blue));
        var skillUseController = new SkillUsecase(pickChampions, CreateSkillRunner());
        var skills = new SlotStorage<Skill>();
        skills.AddSlot(Team.Blue, skill);
        var sut = new AI_SkillUseAgent(skills, skillUseController);

        sut.UseSkill(BlueZeroSlot);

        Assert.AreEqual(10, pickChampions.GetSlot(BlueZeroSlot).Status.Stat.Attack);
    }
}
