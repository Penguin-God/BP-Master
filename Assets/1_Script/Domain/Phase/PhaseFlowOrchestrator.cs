
public interface IPhaseEvent
{
    void Dispatch(GamePhase phase, Team turn);
}

public class PhaseFlowOrchestrator
{
    readonly PhaseAdvancer _phaseFlow;
    readonly IPhaseEvent _dispatcher;

    public GameFlowData CurrentFlow => _phaseFlow.CurrentFlow;

    public PhaseFlowOrchestrator(PhaseData[] phaseDatas, IPhaseEvent dispatcher)
    {
        _phaseFlow = new PhaseAdvancer(phaseDatas);
        _dispatcher = dispatcher;
    }

    public void Start()
    {
        _phaseFlow.Start();
        Disphatch();
    }

    public void SubmitAction(Team actingTeam)
    {
        if (_phaseFlow.SubmitAction(actingTeam))
             Disphatch();
    }

    void Disphatch() => _dispatcher.Dispatch(_phaseFlow.CurrentFlow.Phase, _phaseFlow.CurrentFlow.Turn);
}
