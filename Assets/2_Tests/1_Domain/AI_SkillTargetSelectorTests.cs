using NUnit.Framework;
using System.Linq;
using static TestHelper;

public class AI_SkillTargetSelectorTests
{
    public RandomSkillTargetSelector CreateSelector() => new RandomSkillTargetSelector();

    [Test]
    public void 요청한_개수만큼_반환하고_원본에서_제거한다()
    {
        var targetSlots = CreateBlueSlots(0, 1, 2, 3, 4);
        int targetCount = 2;
        var sut = CreateSelector();

        var result = sut.SelectRandom(targetSlots.ToList(), targetCount).ToList();

        Assert.AreEqual(targetCount, result.Count);
        CollectionAssert.AllItemsAreUnique(result);
        CollectionAssert.IsSubsetOf(result, targetSlots);
    }

    [Test]
    [TestCase(TargetRange.Double, 2)]
    [TestCase(TargetRange.All, 4)]
    public void 스킬을_이용해_타겟_범위_계산_후_선택(TargetRange range, int resultCount)
    {
        const int TARGET_COUNT = 4;
        var countCalculator = new TargetCountCalculator(TARGET_COUNT, TARGET_COUNT);
        var sut = new RandomSkillTargetSelector();

        var result = sut.SelectSkillTargets(Team.Blue, CreateValueSkill(StatType.Attack, 100, rule: new SkillTargetRule(Side.Opponent, range)), countCalculator);

        Assert.AreEqual(resultCount, result.Count());
        CollectionAssert.AllItemsAreUnique(result);
        Assert.IsTrue(result.All(x => x.Team == Team.Red));
    }

    [Test]
    public void All은_전부_반환()
    {
        const int TARGET_COUNT = 4;
        var countCalculator = new TargetCountCalculator(TARGET_COUNT, TARGET_COUNT);
        var sut = new RandomSkillTargetSelector();

        var result = sut.SelectSkillTargets(Team.Blue, CreateValueSkill(StatType.Attack, 100, rule: AllRule), countCalculator);

        Assert.AreEqual(8, result.Count());
        CollectionAssert.AllItemsAreUnique(result);
    }

    [Test]
    public void 스킬_2개는_범위_합치기()
    {
        const int TARGET_COUNT = 4;
        var countCalculator = new TargetCountCalculator(TARGET_COUNT, TARGET_COUNT);
        var sut = new RandomSkillTargetSelector();

        var result = sut.SelectSkillTargets(Team.Blue, CreateSkill(CreateValueSkillData(StatType.Attack, 100, rule: OpponentAllRule), CreateValueSkillData(StatType.Attack, 100, rule: SelfAllRule)), countCalculator);

        Assert.AreEqual(8, result.Count());
        CollectionAssert.AllItemsAreUnique(result);
    }
}
