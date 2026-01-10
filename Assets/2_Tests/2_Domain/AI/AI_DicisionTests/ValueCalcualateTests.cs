using NUnit.Framework;
using static TestHelper;

public class ValueCalcualateTests
{
    [Test]
    public void 스탯_숙련도_스킬점수가_모두_합산되어_반환된다()
    {
        const int CHAMP_ID = 1;
        const int SKILL_VALUE = 100;

        var originSlots = new SlotStorage<ChampionStatus>();
        originSlots.AddSlot(Team.Blue, CreateStatus());

        // 스킬: 공격력 100 증가 (SelfAll) -> 점수 100 (아군 1명 기준)
        var skillData = CreateValueSkillData(SkillType.AttackChanger, SKILL_VALUE, rule: SelfAllRule);
        var champion = CreateChampion(CHAMP_ID, att: 10, skillData: skillData);

        var statCalculator = new ChampionStatValueCalculator(speedValue: 1);

        var previewer = new SkillPreviewer();

        var masteryCollection = new MasteryCollection(new[] { new ChampionMastery(CHAMP_ID, 10) });

        // SUT 생성 (Blue팀 기준)
        var sut = new ChampionValueCalculator(statCalculator, new SkillValueCalculator(previewer, originSlots), masteryCollection, Team.Blue);

        // Act
        int score = sut.Calculate(champion);

        // 예상 점수 합산:
        // 1. 스탯 점수: 10
        // 2. 숙련도 점수: 10 * 2 = 20
        // 3. 스킬 점수: 100 (아군 1명에게 공 100 증가)
        // 총합: 130
        Assert.AreEqual(130, score);
    }

    [Test]
    public void 적군에게_이득을_주는_스킬은_점수가_차감된다()
    {
        // Arrange
        const int CHAMP_ID = 1;
        var originSlots = new SlotStorage<ChampionStatus>();
        originSlots.AddSlot(Team.Red, CreateStatus()); // 적군 1명

        // 적군(Opponent)에게 공격력 100을 주는 트롤링 스킬
        var skillData = CreateValueSkillData(SkillType.AttackChanger, 100, rule: OpponentAllRule);
        var champion = CreateChampion(CHAMP_ID, att: 0, def: 0, speed: 0, skillData);

        var statCalculator = new ChampionStatValueCalculator(0);
        var previewer = new SkillPreviewer();
        var masteryCollection = new MasteryCollection(new ChampionMastery[0]);

        var sut = new ChampionValueCalculator(statCalculator, new SkillValueCalculator(previewer, originSlots), masteryCollection, Team.Blue);

        // Act
        int score = sut.Calculate(champion);

        // Assert
        // 내 팀(Blue) 기준에서 적(Red)의 스탯이 올랐으므로 점수는 마이너스여야 함
        Assert.AreEqual(-100, score);
    }
}
