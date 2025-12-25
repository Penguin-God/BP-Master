
using System;

public class PickHandler
{
    readonly ChampionCatalog championCatalog;
    public readonly PickSlotFacade PickSlotFacade = new();
    readonly GameBanPickStorage gameBanPickStorage;
    public event Action<int> OnPick;
    public event Action<Champion> OnChampionPick;

    public PickHandler(ChampionCatalog championCatalog, GameBanPickStorage gameBanPickStorage)
    {
        this.championCatalog = championCatalog;
        this.gameBanPickStorage = gameBanPickStorage;
    }

    public void Pick(Team team, int id)
    {
        // if(gameBanPickStorage.CanSelected(id) == false) throw new Exception($"픽할 수 없는 ID : {id}");
        var champion = championCatalog.GetChampion(id);
        PickSlotFacade.Add(team, champion);
        OnPick?.Invoke(id);
        OnChampionPick?.Invoke(champion);
    }
}
