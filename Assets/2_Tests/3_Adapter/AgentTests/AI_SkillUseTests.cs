using NUnit.Framework;
using System.Collections.Generic;
using static TestHelper;

public class AI_SkillUseTests
{
    [Test]
    public void UseSkill_호출하면_주입된_타겟에게_스킬이_적용되어야_함()
    {
        var pickChampions = new SlotStorage<PickChampion>();
        var skill = CreateValueSkill(StatType.Attack, 10, rule: SelfTriple);
        pickChampions.AddSlot(Team.Blue, new PickChampion(0, skill, CreateStatus(), Team.Blue));

        var skillUseController = new SkillUsecase(pickChampions, CreateSkillRunner());
        var skills = new SlotStorage<Skill>();
        skills.AddSlot(Team.Blue, skill);
        var stubSelector = new StubTargetSelector(new[] { BlueZeroSlot });

        var sut = new AI_SkillExecutionUseCase(skills, skillUseController, stubSelector);

        sut.UseSkill(BlueZeroSlot);

        Assert.AreEqual(10, pickChampions.GetSlot(BlueZeroSlot).Status.Stat.Attack);
    }
    

    private class StubTargetSelector : ISkillTargetSelector
    {
        private readonly IEnumerable<SlotData> _fixedTargets;
        public StubTargetSelector(IEnumerable<SlotData> targets) => _fixedTargets = targets;

        public IEnumerable<SlotData> SelectSkillTargets(Team team, Skill skill, TargetCountCalculator count) => _fixedTargets;
    }
}
