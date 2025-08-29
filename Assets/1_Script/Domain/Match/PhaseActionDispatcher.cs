
public class PhaseActionDispatcher
{
    IActionHandler _blue;
    IActionHandler _red;
    public PhaseActionDispatcher(IActionHandler blueHandler, IActionHandler redHandler)
    {
        this._blue = blueHandler;
        this._red = redHandler;
    }

    public void ProgressGame(GameFlowData flow)
    {
        switch (flow.Turn)
        {
            case Team.Blue: OnRequestAction(_blue, flow); break;
            case Team.Red: OnRequestAction(_red, flow); break;
            case Team.All:
                OnRequestAction(_blue, flow);
                OnRequestAction(_red, flow);
                break;
        }
    }

    public void OnRequestAction(IActionHandler actionHandler, GameFlowData gameFlow)
    {
        switch (gameFlow.Phase)
        {
            case GamePhase.Ban: actionHandler.OnRequestBan(gameFlow.Turn); break;
            case GamePhase.Pick: actionHandler.OnRequestPick(gameFlow.Turn); break;
            case GamePhase.Swap: actionHandler.OnRequestSwap(gameFlow.Turn); break;
            case GamePhase.Active: actionHandler.OnRequestActive(gameFlow.Turn); break;
        }
    }
}
