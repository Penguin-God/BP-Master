public class AI_BattleSimulator
{
    readonly PhaseManager phaseManager;
    readonly PhaseEventDispatcher phaseEventDispatcher;
    public AI_BattleSimulator(PhaseManager phaseManager, PhaseEventDispatcher phaseEventDispatcher)
    {
        this.phaseManager = phaseManager;
        this.phaseEventDispatcher = phaseEventDispatcher;
    }

    public void RunBanPick(AI_BanPickAgent blue, AI_BanPickAgent red)
    {
        phaseEventDispatcher.OnPhaseBan += blue.Ban;
        phaseEventDispatcher.OnPhasePick += blue.Pick;

        phaseEventDispatcher.OnPhaseBan += red.Ban;
        phaseEventDispatcher.OnPhasePick += red.Pick;

        phaseManager.Start();
    }

    public void RunSkill(AI_SkillAgent blue, AI_SkillAgent red)
    {
        phaseEventDispatcher.OnPhaseSkill += blue.UseSkill;
        phaseEventDispatcher.OnPhaseSkill += red.UseSkill;
    }
}
