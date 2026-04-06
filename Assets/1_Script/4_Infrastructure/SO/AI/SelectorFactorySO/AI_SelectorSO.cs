using UnityEngine;

public abstract class AI_SelectorSO : ScriptableObject
{
    protected MasteryStatCollection masteryManager;
    protected Team team;
    protected ChampionCatalog championCatalog;
    protected SlotStorage<ChampionStatus> statusSlots;
    public void Init(Team team, ChampionCatalog catalog, MasteryStatCollection masteryManager, SlotStorage<ChampionStatus> statusSlots)
    {
        this.masteryManager = masteryManager;
        this.team = team;
        this.championCatalog = catalog;
        this.statusSlots = statusSlots;
    }

    public abstract IChampionSelector CreateBanSelector();
    public abstract IChampionSelector CreatePickSelector();
}
