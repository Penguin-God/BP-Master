using NUnit.Framework;
using System;
using static TestHelper;

public class BanPickHandlerTests
{
    [Test]
    public void 밴하면_저장소에_등록_후_이벤트_발생()
    {
        var sut = CreateSut(1);
        int banId = 0;
        sut.BanPickEventDispatcher.OnBan += (id) => banId = id;

        sut.SaveSelect(new GameFlowData(GamePhase.Ban, Team.Blue), 1);

        Assert.AreEqual(1, banId);
    }

    [Test]
    public void 픽하면_저장소와_퍼사드에_데이터_등록_후_이벤트_발생()
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
        sut.SaveSelect(new GameFlowData(GamePhase.Pick, Team.Blue), CHAMP_ID);

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
