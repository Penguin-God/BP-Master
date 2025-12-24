
public class TimeoutHandler
{
    GameBanPickStorage storage;
    PhaseAdvancer phaseAdvancer;

    public TimeoutHandler(PhaseAdvancer phaseAdvancer, GameBanPickStorage storage)
    {
        this.storage = storage;
        this.phaseAdvancer = phaseAdvancer;
    }

    public void Execute()
    {
        GameFlowData flow = phaseAdvancer.CurrentFlow;

        if (flow.Phase == GamePhase.Pick) storage.SaveSelect(new SelectInfo(flow.Turn, SelectType.Pick, GetRandomSelect()));
        else if (flow.Phase == GamePhase.Ban) storage.SaveSelect(new SelectInfo(flow.Turn, SelectType.Ban, GetRandomSelect()));
    }

    int GetRandomSelect() => RandomUtil.DrawRandom(storage.SelectableIds);
}
