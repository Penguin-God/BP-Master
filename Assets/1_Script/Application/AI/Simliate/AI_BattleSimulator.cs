using System.Collections.Generic;

public readonly struct MatchInfo
{
    public readonly Team Winner;
}

public class AI_BattleSimulator
{
    readonly PhaseManager phaseManager;
    readonly PhaseEventDispatcher phaseEventDispatcher;
    public AI_BattleSimulator(PhaseManager phaseManager, PhaseEventDispatcher phaseEventDispatcher)
    {
        this.phaseManager = phaseManager;
        this.phaseEventDispatcher = phaseEventDispatcher;
    }

    public IEnumerable<MatchInfo> Run(int battleCount)
    {
        return null;
    }

    public MatchInfo Run(AI_Agent blue, AI_Agent red)
    {
        return default;
    }
}
