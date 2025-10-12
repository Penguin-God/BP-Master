using System;

public class PhaseEventDispatcher
{
    public event Action<GameFlowData> OnGameProgress;
    public event Action<Team> OnPhaseBan;
    public event Action<Team> OnPhasePick;
    public event Action<Team> OnPhaseSwap;
    public event Action<Team> OnPhaseTrait;
    public event Action OnPhaseDone;

    public void Dispatch(GamePhase phase, Team turn)
    {
        if (turn == Team.All)
        {
            Raise(phase, Team.Blue);
            Raise(phase, Team.Red);
        }
        else
            Raise(phase, turn);
    }

    void Raise(GamePhase phase, Team turn)
    {
        OnGameProgress?.Invoke(new GameFlowData(phase, turn));
        switch (phase)
        {
            case GamePhase.Ban: OnPhaseBan?.Invoke(turn); break;
            case GamePhase.Pick: OnPhasePick?.Invoke(turn); break;
            case GamePhase.Swap: OnPhaseSwap?.Invoke(turn); break;
            case GamePhase.Trait: OnPhaseTrait?.Invoke(turn); break;
            case GamePhase.Done: OnPhaseDone?.Invoke(); break;
        }
    }
}
