
public class PhaseActionDispatcher
{
    IActionHandler _blue;
    IActionHandler _red;
    public PhaseActionDispatcher(IActionHandler blueHandler, IActionHandler redHandler)
    {
        this._blue = blueHandler;
        this._red = redHandler;
    }

    public void OnRequestAction(GameFlowData flow)
    {
        switch (flow.Turn)
        {
            case Team.Blue: OnRequestAction(_blue, flow); break;
            case Team.Red: OnRequestAction(_red, flow); break;
            case Team.All:
                OnRequestAction(_blue, new GameFlowData(flow.Phase, Team.Blue));
                OnRequestAction(_red, new GameFlowData(flow.Phase, Team.Red));
                break;
        }
    }

    void OnRequestAction(IActionHandler actionHandler, GameFlowData gameFlow)
    {
        switch (gameFlow.Phase)
        {
            case GamePhase.Ban: actionHandler.OnRequestBan(gameFlow.Turn); break;
            case GamePhase.Pick: actionHandler.OnRequestPick(gameFlow.Turn); break;
            case GamePhase.Swap: actionHandler.OnRequestSwap(gameFlow.Turn); break;
            case GamePhase.Trait: actionHandler.OnRequestActive(gameFlow.Turn); break;
        }
    }
}
