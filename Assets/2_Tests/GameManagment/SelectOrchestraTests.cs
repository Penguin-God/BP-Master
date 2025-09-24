using NUnit.Framework;

public class SelectOrchestraTests
{
    PickTableRegistry sut;

    [SetUp]
    public void SetUp()
    {
        var champ = new Champion(3, "삼", new ChampionStatData(10, 20, 30), default, null);
        ChampionCatalog championCatalog = new ChampionCatalog(new Champion[] { champ });
        sut = new PickTableRegistry(championCatalog);
    }

    [Test]
    public void 선택_시_그에_맞는_챔피언_및_상태_등록()
    {
        sut.Pick(Team.Blue, 3);

        Assert.AreEqual(new ChampionStatData(10, 20, 30), sut.GetStatus(TestHelper.CreateBlueSlot(0)).StatData);
        Assert.AreEqual("삼", sut.GetChampion(TestHelper.CreateBlueSlot(0)).Name);
    }
}
