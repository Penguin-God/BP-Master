
public class PhaseFlowOrchestrator
{
    readonly PhaseAdvancer phaseAdvancer;
    readonly IPhaseEvent dispatcher;
    readonly TeamPhaseEntryDispatcher entryDispatcher;

    public GameFlowData CurrentFlow => phaseAdvancer.CurrentFlow;

    public PhaseFlowOrchestrator(PhaseAdvancer phaseAdvancer, IPhaseEvent dispatcher, TeamPhaseEntryDispatcher teamPhaseEntryDispatcher)
    {
        this.phaseAdvancer = phaseAdvancer;
        this.dispatcher = dispatcher;
        entryDispatcher = teamPhaseEntryDispatcher;
    }

    public void Start()
    {
        phaseAdvancer.Start();
        NotifyNewFlow(CurrentFlow);
    }

    public void SubmitAction(Team actingTeam)
    {
        if (phaseAdvancer.TryAdvance(actingTeam))
            NotifyNewFlow(CurrentFlow);
    }

    void NotifyNewFlow(GameFlowData flow)
    {
        entryDispatcher.EnterPhase(flow);
        dispatcher.Dispatch(flow.Phase, flow.Turn);
    }
}
