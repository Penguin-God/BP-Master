using System.Collections.Generic;

public class PhaseFlowOrchestrator
{
    readonly PhaseAdvancer phaseAdvancer;
    readonly IPhaseEvent dispatcher;
    readonly TeamPhaseEntryDispatcher entryDispatcher;

    public GameFlowData CurrentFlow => phaseAdvancer.CurrentFlow;
    public System.Action OnGameEnd;

    public PhaseFlowOrchestrator(IEnumerable<PhaseData> phaseDatas, IPhaseEvent dispatcher, TeamPhaseEntryDispatcher teamPhaseEntryDispatcher)
    {
        this.phaseAdvancer = new PhaseAdvancer(phaseDatas);
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
        if (flow.Phase == GamePhase.Done) OnGameEnd?.Invoke();
    }
}
