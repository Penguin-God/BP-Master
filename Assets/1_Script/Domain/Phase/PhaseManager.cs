using System;
using System.Collections.Generic;

public enum GamePhase { Ban, Pick, Swap, Trait, Done }

public class PhaseData
{
    readonly public GamePhase GamePhase;
    readonly public Phase Phase;

    public PhaseData(GamePhase gamePhase, Phase phase)
    {
        GamePhase = gamePhase;
        Phase = phase;
    }
}

public readonly struct GameFlowData
{
    public readonly GamePhase Phase;
    public readonly Team Turn;

    public GameFlowData(GamePhase phase, Team turn)
    {
        Phase = phase;
        Turn = turn;
    }
}

public class PhaseManager
{
    public event Action<Team> OnPhaseBan;
    public event Action<Team> OnPhasePick;
    public event Action<Team> OnPhaseSwap;
    public event Action<Team> OnPhaseTrait;
    public event Action OnPhaseDone;

    readonly Queue<PhaseData> _phases;
    PhaseData _current;
    public Team CurrentTurn => CurrentFlow.Turn;
    public GameFlowData CurrentFlow { get; private set; }
    readonly HashSet<Team> _submittedInAll = new();

    public event Action<GameFlowData> OnFlowChanged;

    private readonly PhaseEventDispatcher _dispatcher;

    public PhaseManager(PhaseData[] phaseDatas, PhaseEventDispatcher dispatcher)
    {
        _phases = new Queue<PhaseData>(phaseDatas);
        // Done은 마지막 고정
        _phases.Enqueue(new PhaseData(GamePhase.Done, new Phase(new[] { Team.All })));
        _dispatcher = dispatcher;
    }

    public PhaseManager(PhaseData[] phaseDatas)
    {
        _phases = new Queue<PhaseData>(phaseDatas);
        _phases.Enqueue(new PhaseData(GamePhase.Done, new Phase(new[] { Team.All })));
    }

    public void Start()
    {
        _current = _phases.Dequeue();
        Advance();
    }

    public void SubmitAction(Team actingTeam)
    {
        if (_current.GamePhase == GamePhase.Done) return;

        if (CurrentTurn == Team.All)
        {
            _submittedInAll.Add(actingTeam);
            if (_submittedInAll.Contains(Team.Blue) && _submittedInAll.Contains(Team.Red))
            {
                Advance();
                _submittedInAll.Clear();
            }
        }
        else if (actingTeam == CurrentTurn) Advance();
    }

    void Advance()
    {
        if (_current.Phase.IsDone) _current = _phases.Dequeue();

        CurrentFlow = new GameFlowData(_current.GamePhase, _current.Phase.GetNext());

        OnFlowChanged?.Invoke(CurrentFlow);
        _dispatcher.Dispatch(CurrentFlow.Phase, CurrentFlow.Turn);
    }
}
