using NUnit.Framework;
using System.Linq;
using static TestHelper;

public class SkillTargetSelectorsTests
{
    [Test]
    public void 요청한_개수만큼_랜덤_선택()
    {
        var targetSlots = CreateBlueSlots(0, 1, 2, 3, 4);
        int targetCount = 2;
        var sut = new RandomSkillTargetSelector();

        var result = sut.SelectTargets(targetSlots, targetCount, null).ToList();

        Assert.AreEqual(targetCount, result.Count);
        CollectionAssert.AllItemsAreUnique(result);
        CollectionAssert.IsSubsetOf(result, targetSlots);
    }

    [Test]
    public void 지정된_스탯_순으로_정렬후_뽑음()
    {
        var targetSlots = CreateBlueSlots(0, 1, 2, 3, 4);
        var statusSlots = new SlotStorage<ChampionStatus>();
        statusSlots.AddSlots(Team.Blue, new ChampionStatus[] { CreateStatus(100), CreateStatus(200), CreateStatus(500), CreateStatus(400), CreateStatus(300) });
        int targetCount = 2;
        var sut = new HighStatTargetSelector(statusSlots);

        var result = sut.SelectTargets(targetSlots, targetCount, CreateSkill(CreateConditionFreeSkill(StatType.Attack, targetCount))).ToList();

        CollectionAssert.AreEqual(new SlotData[] { CreateBlueSlot(2), CreateBlueSlot(3) }, result);
    }

    [Test]
    public void 스킬이_없으면_타겟도_비어있는거_반환()
    {
        var targetSlots = CreateBlueSlots(0, 1, 2, 3, 4);
        var statusSlots = new SlotStorage<ChampionStatus>();
        statusSlots.AddSlots(Team.Blue, new ChampionStatus[] { CreateStatus(100), CreateStatus(200), CreateStatus(500), CreateStatus(400), CreateStatus(300) });
        int targetCount = 2;
        var sut = new HighStatTargetSelector(statusSlots);

        var result = sut.SelectTargets(targetSlots, targetCount, CreateSkill()).ToList();

        CollectionAssert.AreEqual(new SlotData[] {}, result);
    }
}
