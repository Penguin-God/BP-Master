using System.Collections.Generic;

public interface IPhaseEvent
{
    void Dispatch(GamePhase phase, Team turn);
}

public class PhaseFlowOrchestrator
{
    readonly PhaseAdvancer phaseAdvancer;
    readonly IPhaseEvent dispatcher;
    readonly TeamPhaseEntryDispatcher entryDispatcher;

    public GameFlowData CurrentFlow => phaseAdvancer.CurrentFlow;

    public PhaseFlowOrchestrator(IEnumerable<PhaseData> phaseDatas, IPhaseEvent dispatcher, TeamPhaseEntryDispatcher teamPhaseEntryDispatcher)
    {
        this.phaseAdvancer = new PhaseAdvancer(phaseDatas);
        this.dispatcher = dispatcher;
        entryDispatcher = teamPhaseEntryDispatcher;
    }

    public void Start()
    {
        phaseAdvancer.Start();
        AnnouncedGameFlow(CurrentFlow);
    }

    public void SubmitAction(Team actingTeam)
    {
        if (phaseAdvancer.TryAdvance(actingTeam))
            AnnouncedGameFlow(CurrentFlow);
    }

    void AnnouncedGameFlow(GameFlowData flow)
    {
        entryDispatcher.EnterPhase(flow);
        dispatcher.Dispatch(flow.Phase, flow.Turn);
    }
}
