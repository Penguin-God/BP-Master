using System;

public readonly struct MatchInfo
{
    public readonly Team Winner;
    public readonly int BlueScore;
    public readonly int RedScore;
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

public class AI_SimulateMain
{
    PhaseManager phaseManager;
    PhaseEventDispatcher phaseEventDispatcher;
    readonly int TeamSize;
    Champion[] champions;

    public AI_SimulateMain(PhaseData[] phases, int teamSize, Champion[] champions)
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
