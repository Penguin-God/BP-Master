

public static class SkillRunnerFactory
{
    public static SkillRunner CreateRunner(PhaseActionEventDispatcher phaseActionEventDispatcher, PhaseEventDispatcher phaseEventDispatcher)
    {
        return new SkillRunner(new SkillActionFactory(phaseActionEventDispatcher, phaseEventDispatcher), new SkillCondtionFactory());
    }
}
