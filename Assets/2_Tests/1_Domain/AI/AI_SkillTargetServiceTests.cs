using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using static TestHelper;

public class AI_SkillTargetSelectorTests
{
    class FakeTargetSelector : ISkillTargetSelector
    {
        public IEnumerable<SlotData> SelectTargets(IEnumerable<SlotData> candidates, int count, Skill skill) => candidates.Take(count);
    }

    SkillTargetService CreateSut() => new SkillTargetService(new FakeTargetSelector());

    [Test]
    [TestCase(TargetRange.Double, 2)]
    [TestCase(TargetRange.All, 4)]
    public void 범위_내에서_개수만큼_타겟_선택해야_함(TargetRange range, int resultCount)
    {
        const int TARGET_COUNT = 4;
        var countCalculator = new TargetCountCalculator(TARGET_COUNT, TARGET_COUNT);
        var sut = CreateSut();

        var result = sut.GetTargets(Team.Blue, CreateValueSkill(StatType.Attack, 100, rule: new SkillTargetRule(Side.Opponent, range)), countCalculator);

        Assert.AreEqual(resultCount, result.Count());
        CollectionAssert.AllItemsAreUnique(result);
        Assert.IsTrue(result.All(x => x.Team == Team.Red));
    }

    [Test]
    public void All은_전부_반환()
    {
        const int TARGET_COUNT = 4;
        var countCalculator = new TargetCountCalculator(TARGET_COUNT, TARGET_COUNT);
        var sut = CreateSut();

        var result = sut.GetTargets(Team.Blue, CreateValueSkill(StatType.Attack, 100, rule: AllRule), countCalculator);

        Assert.AreEqual(8, result.Count());
        CollectionAssert.AllItemsAreUnique(result);
    }

    [Test]
    public void 스킬_2개는_범위_합치기()
    {
        const int TARGET_COUNT = 4;
        var countCalculator = new TargetCountCalculator(TARGET_COUNT, TARGET_COUNT);
        var sut = CreateSut();

        var result = sut.GetTargets(Team.Blue, CreateSkill(CreateValueSkillData(StatType.Attack, 100, rule: OpponentAllRule), CreateValueSkillData(StatType.Attack, 100, rule: SelfAllRule)), countCalculator);

        Assert.AreEqual(8, result.Count());
        CollectionAssert.AllItemsAreUnique(result);
    }
}
