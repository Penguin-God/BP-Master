
public class ChampionSelectPresenter
{
    readonly GameBanPickStorage storage;
    int selectId = -1;
    public ChampionSelectPresenter(GameBanPickStorage storage) => this.storage = storage;

    public void SelectChamp(int id) => selectId = id;

    public int NailDownChampion(GameFlowData flow)
    {
        if (storage.SaveSelect(new SelectInfo(flow.Turn, BanPickEnumCaster.PhaseToSelect(flow.Phase), selectId)))
        {
            int result = selectId;
            selectId = -1;
            return result;
        }
        else return -1;
    }
}
