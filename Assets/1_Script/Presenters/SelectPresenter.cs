
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
        if (storage.SaveSelect(new SelectInfo(Turn, BanPickEnumCaster.PhaseToSelect(Phase), selectId)))
        {
            int result = selectId;
            selectId = -1;
            return result;
        }
        else return -1;
    }
}
