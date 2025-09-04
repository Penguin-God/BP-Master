
public class SelectPresenter
{
    readonly GameBanPickStorage storage;
    GameFlowData currentFlowData;
    int selectId = -1;

    public GamePhase Phase => currentFlowData.Phase;
    public Team Turn => currentFlowData.Turn;
    public SelectPresenter(GameBanPickStorage storage) => this.storage = storage;

    public void ChangeFlow(GameFlowData gameFlowData) => currentFlowData = gameFlowData;
    public void SelectChamp(int id) => selectId = id;

    public int NailDownChampion()
    {
        if (selectId == -1) return -1;

        //if (currentFlowData.Phase == GamePhase.Ban) return storage.SaveSelect(new SelectInfo(currentFlowData.Turn, SelectType.Ban, selectId));
        //else if (currentFlowData.Phase == GamePhase.Pick) return storage.SaveSelect(new SelectInfo(currentFlowData.Turn, SelectType.Pick, selectId));
        //else return -1;
        return 0;
    }
}
