
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
        storage.SaveSelect(flow, GetRandomSelect());
    }

    int GetRandomSelect() => RandomUtil.DrawRandom(storage.SelectableIds);
}
