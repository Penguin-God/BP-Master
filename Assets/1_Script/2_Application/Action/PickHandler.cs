using System;
using System.Collections.Generic;
using System.Linq;

public class PickHandler
{
    readonly ChampionCatalog championCatalog;
    public readonly PickSlotFacade PickSlotFacade = new();
    readonly BanPickEventDispatcher eventDispatcher;
    public PickHandler(ChampionCatalog championCatalog, BanPickEventDispatcher eventDispatcher)
    {
        this.championCatalog = championCatalog;
        this.eventDispatcher = eventDispatcher;
    }

    public void Pick(SlotData slotData, int id)
    {
        var champion = championCatalog.GetChampion(id);
        PickSlotFacade.Add(slotData.Team, champion);
        eventDispatcher.RaisePick(champion, slotData);
    }
}

public class BanPickHandler
{
    readonly ChampionCatalog championCatalog;
    public readonly PickSlotFacade PickSlotFacade = new();
    readonly BanPickStorage storage;
    public readonly BanPickEventDispatcher BanPickEventDispatcher = new();
    readonly IEnumerable<GamePhase> VaildPhases = new GamePhase[] { GamePhase.Ban, GamePhase.Pick };

    public BanPickHandler(ChampionCatalog championCatalog, BanPickStorage storage)
    {
        this.championCatalog = championCatalog;
        this.storage = storage;
    }

    public void Pick(Team team, int id)
    {
        var champion = championCatalog.GetChampion(id);
        var slotData = storage.Pick(team, id);
        PickSlotFacade.Add(slotData.Team, champion);
        BanPickEventDispatcher.RaisePick(champion, slotData);
    }

    public void Ban(Team team, int id)
    {
        storage.Ban(team, id);
        BanPickEventDispatcher.RasieBan(team, id);
    }


    public void SaveSelect(GameFlowData flow, int selectedId)
    {
        if (storage.CanSelected(selectedId) == false || VaildPhases.Contains(flow.Phase) == false) throw new ArgumentException($"선택 불가능. ID : {selectedId}, Phase : {flow.Phase}");

        if (flow.Phase == GamePhase.Ban) Ban(flow.Turn, selectedId);
        else Pick(flow.Turn, selectedId);
    }
}
