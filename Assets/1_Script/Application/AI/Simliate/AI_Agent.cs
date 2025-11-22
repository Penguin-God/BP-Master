

public class AI_Agent
{
    readonly AI_BanPickAgent banPickAgent;
    readonly AI_SkillAgent skillAgent;

    public AI_Agent(AI_BanPickAgent banPickAgent, AI_SkillAgent skillAgent)
    {
        this.banPickAgent = banPickAgent;
        this.skillAgent = skillAgent;
    }
}
