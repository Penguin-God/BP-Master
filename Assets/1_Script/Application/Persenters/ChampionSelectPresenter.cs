
public class ChampionSelectPresenter
{
    readonly GameBanPickStorage storage;
    int selectId = -1;
    public ChampionSelectPresenter(GameBanPickStorage storage) => this.storage = storage;

    public void SelectChamp(int id) => selectId = id;
    public void NailDownChampion(GameFlowData flow) => storage.SaveSelect(flow, selectId);
}
