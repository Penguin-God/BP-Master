using NUnit.Framework;
using System.Linq;

public class RosterCreateTests
{
    [Test]
    public void 랜덤으로_숙련도_뽑아서_로스터_생성()
    {
        ChampionCatalog catalog = new ChampionCatalog(TestHelper.CreateFiveChamps());
        MasteryDrawer drawer = new(catalog);
        

        RandomRosterFactory sut = new (drawer);
        int rosterCount = 5;
        var levels = new int[] { 5, 10 };

        SlotStorage<ProGamer> result = sut.CreateRoster(rosterCount, levels);

        Assert.AreEqual(5, result.GetTeam(Team.Blue).Count());
        Assert.AreEqual(5, result.GetTeam(Team.Red).Count());

        // 레벨 검증
        Assert.AreEqual(5, result.GetSlot(TestHelper.CreateRedSlot(0)).AllMasteries.ToArray()[0].Level);
        Assert.AreEqual(10, result.GetSlot(TestHelper.CreateRedSlot(0)).AllMasteries.ToArray()[1].Level);
    }
}
