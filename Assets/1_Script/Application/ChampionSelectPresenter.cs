
public class ChampionSelectPresenter
{
    readonly GameBanPickStorage storage;
    int selectId = -1;
    public ChampionSelectPresenter(GameBanPickStorage storage) => this.storage = storage;

    public void SelectChamp(int id) => selectId = id;

    public void NailDownChampion(GameFlowData flow)
    {
        if (storage.CanSelected(selectId) == false) throw new System.Exception($"선택 불가 ID : {selectId}");

        if (flow.Phase == GamePhase.Pick) storage.SaveSelect(new SelectInfo(flow.Turn, SelectType.Pick, selectId));
        else if(flow.Phase == GamePhase.Ban) storage.SaveSelect(new SelectInfo(flow.Turn, SelectType.Ban, selectId));
        else throw new System.Exception($"선택 불가 ID : {selectId}");
    }
}
