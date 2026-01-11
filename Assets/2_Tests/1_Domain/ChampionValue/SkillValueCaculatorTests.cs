using NUnit.Framework;
using static TestHelper;

public class SkillValueCaculatorTests
{
    //[Test]
    //public void 스킬_가치_계산()
    //{
    //    const int SKILL_VALUE = 100;

    //    var originSlots = new SlotStorage<ChampionStatus>();
    //    originSlots.AddSlot(Team.Blue, CreateStatus());

    //    // 스킬: 공격력 100 증가 (SelfAll) -> 점수 100 (아군 1명 기준)
    //    var skillData = CreateValueSkillData(SkillType.AttackChanger, SKILL_VALUE, rule: SelfAllRule);
    //    var champion = CreateChampion(id:1, skillData: skillData);
    //    var previewer = new SkillPreviewer();

    //    var sut = new SkillValueCalculator(previewer, originSlots);

    //    GameScoreInfo result = sut.Calculate(Team.Blue, champion);

    //    Assert.AreEqual(100, result.Blue.Att);
    //    Assert.AreEqual(0, result.Red.Att);
    //}

    [Test]
    public void 챔피언에_숙련도_적용_후_스탯과_스킬까지_적용한_값_반환()
    {
        var originSlots = CreateOneSlotStatus();

        var skillData = CreateValueSkillData(SkillType.AttackChanger, value: 100, rule: SelfAllRule);
        var champion = CreateChampion(id: 1, att:100, skillData: skillData);
        var previewer = new SkillPreviewer();

        var sut = new SkillValueCalculator(previewer, CreateMasteryCollection(new ChampionMastery(1, 10)));

        GameScoreInfo result = sut.Calculate(Team.Blue, champion, originSlots);

        Assert.AreEqual(210, result.Blue.Att);
        Assert.AreEqual(10, result.Blue.Def);
        Assert.AreEqual(0, result.Red.Att);
    }
}
