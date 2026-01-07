using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using static TestHelper;

public class ChampionValueSelectTests
{
    [Test]
    public void 가장_가치가_높은_챔피언_픽()
    {
        const Team Team = Team.Blue;
        var statusSlots = CreateTwoSlotStatus();
        ChampionCatalog catalog = CreateCaltalog(CreateChamp(1, 100), CreateChamp(2, 0), CreateChamp(3, 0));
        ChampionStatValueCalculator statCalculator = new ChampionStatValueCalculator(speedValue: 10);
        MasteryCollection masteryCollection = new MasteryCollection(new ChampionMastery[] { new ChampionMastery(1, 10) });
        SkillValueCalculator skillValueCalculator = new SkillValueCalculator(new SkillPreviewer(), statusSlots);

        // 계산식(스탯 밸류+ 마스터리 레벨 * 2 + 스킬 밸류)
        ValuePick sut = new ValuePick(catalog, new ChampionValueCalculator(statCalculator, skillValueCalculator, masteryCollection, Team));

        int result = sut.Pick(new HashSet<int>() { 1, 2, 3 });

        Assert.AreEqual(1, result);
    }
    Champion CreateChamp(int id, int value) => CreateChampion(id, skillData: CreateValueSkillData(SkillType.AttackChanger, value, rule: SelfAllRule));
    ChampionCatalog CreateCaltalog(params Champion[] champions) => new ChampionCatalog(champions);

    [Test]
    public void 점수가_높은_순서대로_정렬하여_반환한다()
    {
        // Arrange
        const Team Team = Team.Blue;
        var statusSlots = CreateTwoSlotStatus();

        // 챔피언 3명 생성 (ID: 10=점수100, ID: 20=점수300, ID: 30=점수50)
        var c1 = CreateChamp(10, 100);
        var c2 = CreateChamp(20, 300); // 1등 예상
        var c3 = CreateChamp(30, 50);

        ChampionCatalog catalog = CreateCaltalog(c1, c2, c3);

        // 계산기 로직 (간소화된 설정)
        var statCalc = new ChampionStatValueCalculator(0);
        var mastCalc = new MasteryCollection(new ChampionMastery[0]);
        var skillCalc = new SkillValueCalculator(new SkillPreviewer(), statusSlots);
        var valueCalculator = new ChampionValueCalculator(statCalc, skillCalc, mastCalc, Team);

        var sut = new ChampionRanker(catalog, valueCalculator);

        // Act
        var result = sut.GetChampionRank(new HashSet<int> { 10, 20, 30 }).ToList();

        // Assert
        Assert.AreEqual(3, result.Count);

        // 1등 확인 (ID 20)
        Assert.AreEqual(20, result[0].Id);

        // 2등 확인 (ID 10)
        Assert.AreEqual(10, result[1].Id);

        // 3등 확인 (ID 30)
        Assert.AreEqual(30, result[2].Id);
    }
}
