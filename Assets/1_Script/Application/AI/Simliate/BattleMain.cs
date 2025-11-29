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

    public BattleMain(PhaseData[] phases, int teamSize, Champion[] champions, IPhaseAgent blue, IPhaseAgent red)
    {
        phaseEventDispatcher = new PhaseEventDispatcher();
        phaseManager = new PhaseManager(phases, phaseEventDispatcher);
        this.TeamSize = teamSize;
        this.champions = champions;
    }

    public MatchInfo Run()
    {
        throw new NotImplementedException();
    }
}
