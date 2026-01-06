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
        var previewer = new SkillPreviewer();

        var sut = new SkillValueCalculator(previewer, originSlots);

        GameStatChangeInfo result = sut.Calculate(Team.Blue, champion);

        Assert.AreEqual(100, result.Blue.Att);
        Assert.AreEqual(0, result.Red.Att);
    }
}
