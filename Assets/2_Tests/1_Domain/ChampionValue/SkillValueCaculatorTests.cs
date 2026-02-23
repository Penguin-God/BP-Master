using NUnit.Framework;
using static TestHelper;

public class SkillValueCaculatorTests
{
    [Test]
    public void 챔피언_스탯_스킬_숙련도_적용한_점수_반환()
    {
        var originSlots = CreateOneSlotStatus();

        var skillData = CreateAttackChangeSkill(value: 100, rule: SelfAllRule); // 픽 점수 100
        var champion = CreateChampion(id: 1, att:100, skillData: skillData);  // 스킬 점수 100
        var previewer = new SkillPreviewer();

        var sut = new ChampionValueCalculator(previewer, CreateMasteryApplier(new ChampionMastery(1, 10))); // 숙련도 점수 att, def 10

        GameScoreInfo result = sut.Calculate(Team.Blue, champion, originSlots);

        Assert.AreEqual(210, result.Blue.Att);
        Assert.AreEqual(10, result.Blue.Def);
        Assert.AreEqual(0, result.Red.Att);
    }

    [Test]
    public void 상대_팀에_적용되는_스킬도_계산()
    {
        var originSlots = CreateTwoSlotStatus();

        var skillData = CreateAttackChangeSkill(value: 150, rule: OpponentAllRule); // 스킬 점수 150 X 2
        var champion = CreateChampion(id: 1, att: 100, skillData: skillData); // 내가 픽한 챔프의 스탯은 상대 점수에 반영 X
        
        var sut = new ChampionValueCalculator(new SkillPreviewer(), CreateMasteryApplier());

        GameScoreInfo result = sut.Calculate(Team.Blue, champion, originSlots);

        Assert.AreEqual(300, result.Red.Att);
    }
}
