

public static class SkillRunnerFactory
{
    public static SkillRunner CreateRunner(PhaseActionEventDispatcher phaseActionEventDispatcher, PhaseEventDispatcher phaseEventDispatcher, Team team)
    {
        return new SkillRunner(new SkillActionFactory(phaseActionEventDispatcher, phaseEventDispatcher, team), new SkillCondtionFactory());
    }
}
