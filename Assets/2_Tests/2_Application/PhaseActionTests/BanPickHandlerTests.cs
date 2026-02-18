using NUnit.Framework;
using System;
using static TestHelper;

public class BanPickHandlerTests
{
    [Test]
    public void 픽하면_해당_팀에_챔피언이_슬롯에_추가됨()
    {
        const int CHAMP_ID = 1;
        var champion = new Champion(CHAMP_ID, null, CreateStatus(10, 10, 10));
        var catalog = new ChampionCatalog(new Champion[] { champion });
        var storage = CreateStorage(CHAMP_ID);
        var eventDispathcer = new BanPickEventDispatcher();
        Champion champ = null;
        Team team = Team.Red;

        var sut = new PickHandler(catalog, eventDispathcer);
        eventDispathcer.OnTeamChampionPick += (cham, _team) => (champ, team) = (cham, _team);

        sut.Pick(new SlotData(Team.Blue, 0), CHAMP_ID);

        Assert.IsNotNull(sut.PickSlotFacade.StatusSlots.GetSlot(BlueZeroSlot));
        Assert.AreEqual(10, champ.Status.Stat.Attack);
    }


    [Test]
    public void 픽_호출_시_저장소와_파사드에_데이터가_쌓이고_이벤트가_발생해야_함()
    {
        const int CHAMP_ID = 7;
        var status = CreateStatus(att: 20);
        var champion = new Champion(CHAMP_ID, null, status);

        var catalog = new ChampionCatalog(new[] { champion });
        var storage = new BanPickStorage(new[] { CHAMP_ID });
        var sut = new BanPickHandler(catalog, storage);

        SlotChampion eventResult = null;
        sut.BanPickEventDispatcher.OnChampionPick += (pc) => eventResult = pc;

        // Act
        sut.Pick(Team.Blue, CHAMP_ID);

        // 1. Storage 확인: PickIds에 해당 ID가 들어갔는가?
        Assert.AreEqual(CHAMP_ID, storage.PickIds.GetSlot(BlueZeroSlot));

        // 2. Facade 확인: 챔피언 실체가 슬롯에 추가되었는가?
        Assert.AreSame(champion, sut.PickSlotFacade.ChampionSlots.GetSlot(BlueZeroSlot));

        // 3. Event 확인: PickChampion 객체가 올바른 정보를 담고 전달되었는가?
        Assert.AreEqual(CHAMP_ID, eventResult.Id);
        Assert.AreEqual(Team.Blue, eventResult.SlotData.Team);
        Assert.AreSame(status, eventResult.Status);
    }

    [Test]
    public void 픽_페이즈일_때_정상적으로_픽을_수행해야_함()
    {
        var id = 1;
        var sut = CreateSut(id);
        var flow = new GameFlowData(GamePhase.Pick, Team.Blue);

        sut.SaveSelect(flow, id);

        Assert.IsNotNull(sut.PickSlotFacade.StatusSlots.GetSlot(BlueZeroSlot));
    }

    [Test]
    public void 선택_불가능한_ID를_전달하면_예외()
    {
        var id = 1;
        var sut = CreateSut(id);
        var flow = new GameFlowData(GamePhase.Pick, Team.Blue);
        sut.SaveSelect(flow, 1);

        Assert.Throws<ArgumentException>(() => sut.SaveSelect(flow, id));
    }

    [Test]
    public void 허용되지_않은_페이즈일_경우_예외()
    {
        var id = 1;
        var sut = CreateSut(id);
        var flow = new GameFlowData(GamePhase.Done, Team.Blue);

        Assert.Throws<ArgumentException>(() => sut.SaveSelect(flow, id));
    }

    // --- Helper ---
    BanPickHandler CreateSut(int selectableId)
    {
        var champion = new Champion(selectableId, null, CreateStatus());
        var catalog = new ChampionCatalog(new[] { champion });
        var storage = new BanPickStorage(new[] { selectableId });
        return new BanPickHandler(catalog, storage);
    }
}
