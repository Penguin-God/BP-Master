using UnityEngine;

[CreateAssetMenu(fileName = "SelectorCreatetorSO", menuName = "Scriptable Objects/SelectorCreatetorSO")]
public abstract class AI_SelectorFactory : ScriptableObject
{
    protected MasteryCollection masteryManager;
    protected Team team;
    protected ChampionCatalog championCatalog;
    protected SlotStorage<ChampionStatus> statusSlots;
    public void Init(Team team, ChampionCatalog catalog, MasteryCollection masteryManager, SlotStorage<ChampionStatus> statusSlots)
    {
        this.masteryManager = masteryManager;
        this.team = team;
        this.championCatalog = catalog;
        this.statusSlots = statusSlots;
    }

    public abstract IChampionSelector CreateBanSelector();
    public abstract IChampionSelector CreatePickSelector();
}
