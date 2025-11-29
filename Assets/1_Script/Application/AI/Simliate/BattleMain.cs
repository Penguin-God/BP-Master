using System;

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
    readonly AI_SkillAgent skillAgent;

    public AI_PhaseAgent(AI_BanPickAgent banPickAgent, AI_SkillAgent skillAgent)
    {
        this.banPickAgent = banPickAgent;
        this.skillAgent = skillAgent;
    }

    public void OnBan(Team team) => banPickAgent.Ban(team);
    public void OnPick(Team team) => banPickAgent.Pick(team);
    public void OnSkill(Team team) => skillAgent.UseSkill(team);
}

public class Champion
{
    readonly Skill Skill;
    readonly ChampionStatus Status;

    public Champion(Skill skill, ChampionStatus status)
    {
        Skill = skill;
        Status = status;
    }
}

public class BattleMain
{
    PhaseManager phaseManager;
    PhaseEventDispatcher phaseEventDispatcher;
    readonly int TeamSize;
    Champion[] champions;
    IPhaseAgent blue;
    IPhaseAgent red;
    public BattleMain(PhaseManager phaseManager, PhaseEventDispatcher phaseEventDispatcher, Champion[] champions, IPhaseAgent blue, IPhaseAgent red)
    {
        this.phaseEventDispatcher = phaseEventDispatcher;
        this.phaseManager = phaseManager;
        this.champions = champions;
        this.blue = blue;
        this.red = red;
    }

    public MatchInfo Run()
    {
        SubscribePhaseEvent(blue);
        SubscribePhaseEvent(red);
        phaseManager.Start();
        return default;
    }

    private void SubscribePhaseEvent(IPhaseAgent phaseAgent)
    {
        phaseEventDispatcher.OnPhaseBan += phaseAgent.OnBan;
        phaseEventDispatcher.OnPhasePick += phaseAgent.OnPick;
        phaseEventDispatcher.OnPhaseSkill += phaseAgent.OnSkill;
    }
}
