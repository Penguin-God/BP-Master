using NUnit.Framework;
using static TestHelper;

public class MasteryDrawTests
{
    [Test]
    public void 주어진_레벨을_랜덤_숙련도로_반환()
    {
        var champs = new Champion[]
        {
            CreateChamp(1, "일"),
            CreateChamp(2, "이"),
            CreateChamp(3, "삼"),
            CreateChamp(4, "사"),
            CreateChamp(5, "오"),
        };

        ChampionCatalog catalog = new ChampionCatalog(champs);
        MasteryDrawer sut = new(catalog);
        var levels = new int[] { 5, 10 };

        ChampionMastery[] result = sut.DrawRandoms(levels);

        Assert.AreEqual(5, result[0].Level);
        Assert.AreEqual(10, result[1].Level);

        var validIds = new int[] { 1, 2, 3, 4, 5 };
        CollectionAssert.Contains(validIds, result[0].ChampionId);
        CollectionAssert.Contains(validIds, result[1].ChampionId);
    }
}
