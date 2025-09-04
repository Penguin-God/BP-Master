public class SelectPresenter
{
    readonly GameBanPickStorage storage;
    
    public SelectPresenter(GameBanPickStorage storage) => this.storage = storage;

    public bool SelectChampion(GamePhase phase, Team team, int id)
    {
        if (phase == GamePhase.Ban) return storage.SaveSelect(new SelectInfo(team, SelectType.Ban, id));
        else if (phase == GamePhase.Pick) return storage.SaveSelect(new SelectInfo(team, SelectType.Pick, id));
        else return false;
    }
}
