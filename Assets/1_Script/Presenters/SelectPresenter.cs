using System;

public class SelectPresenter
{
    readonly GameBanPickStorage storage;
    GameFlowData currentFlowData;
    public GamePhase Phase => currentFlowData.Phase;
    public Team Turn => currentFlowData.Turn;
    public SelectPresenter(GameBanPickStorage storage) => this.storage = storage;

    public void ChangeFlow(GameFlowData gameFlowData) => currentFlowData = gameFlowData;

    public bool SelectChampion(int id)
    {
        if (currentFlowData.Phase == GamePhase.Ban) return storage.SaveSelect(new SelectInfo(currentFlowData.Turn, SelectType.Ban, id));
        else if (currentFlowData.Phase == GamePhase.Pick) return storage.SaveSelect(new SelectInfo(currentFlowData.Turn, SelectType.Pick, id));
        else return false;
    }
}
