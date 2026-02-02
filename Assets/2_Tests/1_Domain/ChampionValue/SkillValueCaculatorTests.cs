using NUnit.Framework;
using static TestHelper;

public class SkillValueCaculatorTests
{
    [Test]
    public void 챔피언에_숙련도_적용_후_스탯과_스킬까지_적용한_값_반환()
    {
        var originSlots = CreateOneSlotStatus();

        var skillData = CreateAttackChangeSkill(value: 100, rule: SelfAllRule);
        var champion = CreateChampion(id: 1, att:100, skillData: skillData);
        var previewer = new SkillPreviewer();

        var sut = new ChampionValueApplier(previewer, CreateMasteryApplier(new ChampionMastery(1, 10)));

        GameScoreInfo result = sut.Calculate(Team.Blue, champion, originSlots);

        Assert.AreEqual(210, result.Blue.Att);
        Assert.AreEqual(10, result.Blue.Def);
        Assert.AreEqual(0, result.Red.Att);
    }
}
