using NUnit.Framework;
using static TestHelper;

public class AI_SkillUseTests
{
    [Test]
    public void UseSkill_호출하면_주입된_타겟에게_스킬이_적용되어야_함()
    {
        var pickChampions = new SlotStorage<Champion>();
        var skill = CreateValueSkill(StatType.Attack, 10, rule: SelfTriple);
        pickChampions.AddSlot(Team.Blue, new Champion(0, skill, CreateStatus()));

        var skillUseController = new SkillUsecase(pickChampions, CreateSkillRunner());
 
        var sut = new AI_SkillExecutionUseCase(skillUseController, (status) => status.Stat.Attack); // attack 순으로 정렬

        sut.UseSkill(BlueZeroSlot);

        Assert.AreEqual(10, pickChampions.GetSlot(BlueZeroSlot).Status.Stat.Attack);
    }
}
