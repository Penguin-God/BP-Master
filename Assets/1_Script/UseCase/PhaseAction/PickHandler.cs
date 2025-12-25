
using System;

public class PickHandler
{
    readonly ChampionCatalog championCatalog;
    readonly PickSlotFacade pickSlotFacade;
    readonly GameBanPickStorage gameBanPickStorage;
    public event Action<int> OnPick;
    public event Action<Champion> OnChampionPick;

    public PickHandler(ChampionCatalog championCatalog, PickSlotFacade pickSlotFacade, GameBanPickStorage gameBanPickStorage)
    {
        this.championCatalog = championCatalog;
        this.pickSlotFacade = pickSlotFacade;
        this.gameBanPickStorage = gameBanPickStorage;
    }

    public void Pick(Team team, int id)
    {
        if(gameBanPickStorage.IdIsSelected(id) == false) throw new Exception($"픽할 수 없는 ID : {id}");
        var champion = championCatalog.GetChampion(id);
        pickSlotFacade.Add(team, champion);
        OnPick?.Invoke(id);
        OnChampionPick?.Invoke(champion);
    }
}
