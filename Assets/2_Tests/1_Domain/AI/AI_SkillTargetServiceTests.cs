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

    TargetCountCalculator CreateSut(int blueCount = 4, int redCount = 4) => new (blueCount, redCount);

    [Test]
    [TestCase(TargetRange.Double, 2)]
    [TestCase(TargetRange.All, 4)]
    public void 범위_내에서_개수만큼_타겟_선택해야_함(TargetRange range, int resultCount)
    {
        var result = CreateSut().GetTargets(Team.Blue, CreateValueSkill(StatType.Attack, 100, rule: new SkillTargetRule(Side.Opponent, range)), new FakeTargetSelector());

        Assert.AreEqual(resultCount, result.Count());
        CollectionAssert.AllItemsAreUnique(result);
        Assert.IsTrue(result.All(x => x.Team == Team.Red));
    }

    [Test]
    public void All은_전부_반환()
    {
        var result = CreateSut().GetTargets(Team.Blue, CreateValueSkill(StatType.Attack, 100, rule: AllRule), new FakeTargetSelector());

        Assert.AreEqual(8, result.Count());
        CollectionAssert.AllItemsAreUnique(result);
    }

    [Test]
    public void 스킬_2개는_범위_합치기()
    {
        var result = CreateSut().GetTargets(Team.Blue, CreateSkill(CreateValueSkillData(StatType.Attack, 100, rule: OpponentAllRule), CreateValueSkillData(StatType.Attack, 100, rule: SelfAllRule)), new FakeTargetSelector());

        Assert.AreEqual(8, result.Count());
        CollectionAssert.AllItemsAreUnique(result);
    }
}
