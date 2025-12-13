
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
        if (_phaseFlow.TryAdvance(actingTeam))
             Disphatch();
    }

    void Disphatch() => _dispatcher.Dispatch(_phaseFlow.CurrentFlow.Phase, _phaseFlow.CurrentFlow.Turn);
}


public class PhaseFlowOrchestrator2
{
    readonly PhaseAdvancer phaseAdvancer;
    readonly IPhaseEvent dispatcher;

    public GameFlowData CurrentFlow => phaseAdvancer.CurrentFlow;

    public PhaseFlowOrchestrator2(PhaseAdvancer phaseAdvancer, IPhaseEvent dispatcher)
    {
        this.phaseAdvancer = phaseAdvancer;
        this.dispatcher = dispatcher;
    }

    public void Start()
    {
        phaseAdvancer.Start();
        Disphatch();
    }

    public void SubmitAction(Team actingTeam)
    {
        if (phaseAdvancer.TryAdvance(actingTeam))
        {
            Disphatch();
            // phaseAdvancer.CurrentTurn
        }
    }

    void Disphatch() => dispatcher.Dispatch(phaseAdvancer.CurrentFlow.Phase, phaseAdvancer.CurrentFlow.Turn);
}
