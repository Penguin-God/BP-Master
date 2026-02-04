using NUnit.Framework;
using static TestHelper;

public class PickHandlerTests
{
    [Test]
    public void 픽하면_해당_팀에_챔피언이_슬롯에_추가됨()
    {
        const int CHAMP_ID = 1;
        var champion = new Champion(CHAMP_ID, null, CreateStatus(10, 10, 10));
        var catalog = new ChampionCatalog(new Champion[] { champion });
        var storage = CreateStorage(CHAMP_ID);
        var eventDispathcer = new PhaseActionEventDispatcher();
        Champion champ = null;
        Team team = Team.Red;

        var sut = new PickHandler(catalog, eventDispathcer);
        eventDispathcer.OnChampionPick += (cham, _team) => (champ, team) = (cham, _team);

        sut.Pick(Team.Blue, CHAMP_ID);

        Assert.IsNotNull(sut.PickSlotFacade.StatusSlots.GetSlot(BlueZeroSlot));
        Assert.AreEqual(10, champ.Status.Stat.Attack);
    }
}
