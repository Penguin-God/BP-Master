using NUnit.Framework;
using static TestHelper;

public class SkillValueCaculatorTests
{
    [Test]
    public void 스킬_가치_계산()
    {
        const int SKILL_VALUE = 100;

        var originSlots = new SlotStorage<ChampionStatus>();
        originSlots.AddSlot(Team.Blue, CreateStatus());

        // 스킬: 공격력 100 증가 (SelfAll) -> 점수 100 (아군 1명 기준)
        var skillData = CreateValueSkillData(SkillType.AttackChanger, SKILL_VALUE, rule: SelfAllRule);
        var champion = CreateChampion(id:1, skillData: skillData);

        var statCalculator = new ChampionStatValueCalculator(speedValue: 0);
        var previewer = new SkillPreviewer(Team.Blue, CreateSkillExceutorFactory(), originSlots);
        var deltaCalculator = new SkillApplyDeltaCalculator();

        var sut = new SkillValueCalculator(previewer, statCalculator, deltaCalculator);

        int result = sut.Calculate(Team.Blue, champion, originSlots);

        Assert.AreEqual(100, result);
    }
}
