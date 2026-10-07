

public static class SkillRunnerFactory
{
    public static SkillRunner CreateRunner(BanPickEventDispatcher phaseActionEventDispatcher, PhaseEventDispatcher phaseEventDispatcher)
    {
        return new SkillRunner(new SkillActionFactory(phaseActionEventDispatcher, phaseEventDispatcher), new SkillCondtionFactory());
    }
}
