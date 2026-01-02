using NUnit.Framework;
using System.Linq;
using static TestHelper;

public class AI_SkillTargetSelectorTests
{
    public AI_SkillTargetSelector CreateSelector() => new AI_SkillTargetSelector();

    [Test]
    public void SelectSkillTargets_요청한_개수만큼_반환하고_원본에서_제거한다()
    {
        var targetSlots = CreateBlueSlots(0, 1, 2, 3, 4);
        int targetCount = 2;
        var sut = CreateSelector();

        var result = sut.SelectSkillTargets(targetSlots.ToList(), targetCount).ToList();

        Assert.AreEqual(targetCount, result.Count);
        CollectionAssert.AllItemsAreUnique(result);
        CollectionAssert.IsSubsetOf(result, targetSlots);
    }
}
