using System.Linq;

public readonly struct MatchInfo
{
    public readonly Team Winner;
    public readonly int BlueScore;
    public readonly int RedScore;
}

public interface IPhaseAgent
{
    void OnBan(Team team);
    void OnPick(Team team);
    void OnSkill(Team team);
}

public class AI_PhaseAgent : IPhaseAgent
{
    readonly AI_BanPickAgent banPickAgent;

    public AI_PhaseAgent(AI_BanPickAgent banPickAgent)
    {
        this.banPickAgent = banPickAgent;
    }

    public void OnBan(Team team) => banPickAgent.Ban(team);
    public void OnPick(Team team) => banPickAgent.Pick(team);
    public void OnSkill(Team team) => banPickAgent.Pick(team); //skillAgent.UseSkill(team);
}

public class MatchRunner
{
    readonly GameBanPickStorage banPickStorage;
    readonly PhaseFlowOrchestrator phaseManager;
    readonly PhaseEventDispatcher phaseEventDispatcher;
    readonly ChampionCatalog championCatalog;
    readonly int TeamSize;
    readonly IPhaseAgent blue;
    readonly IPhaseAgent red;
    
    public MatchRunner(PhaseFlowOrchestrator phaseManager, PhaseEventDispatcher phaseEventDispatcher, Champion[] champions, IPhaseAgent blue, IPhaseAgent red)
    {
        banPickStorage = new GameBanPickStorage(champions.Select(x => x.Id));
        championCatalog = new ChampionCatalog(champions);

        this.phaseEventDispatcher = phaseEventDispatcher;
        this.phaseManager = phaseManager;
        this.blue = blue;
        this.red = red;
    }

    public void Run()
    {
        SubscribePhaseEvent(blue);
        SubscribePhaseEvent(red);
        phaseManager.Start();
    }

    void SubscribePhaseEvent(IPhaseAgent phaseAgent)
    {
        phaseEventDispatcher.OnPhaseBan += phaseAgent.OnBan;
        phaseEventDispatcher.OnPhasePick += phaseAgent.OnPick;
        phaseEventDispatcher.OnPhaseSkill += phaseAgent.OnSkill;
    }

    bool initSkill = false;
    void InitSkillPhase()
    {
        if (initSkill) return;
        initSkill = true;


    }
}
