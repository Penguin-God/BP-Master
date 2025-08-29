using System;
using System.Collections.Generic;

public enum GamePhase { Ban, Pick, Swap, Active, Done }

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
    readonly Queue<PhaseData> _phases;
    PhaseData _current;
    Team _currentTurn => CurrentFlow.Turn;
    public GameFlowData CurrentFlow { get; private set; }
    readonly HashSet<Team> _submittedInAll = new();

    public event Action<GameFlowData> OnFlowChanged;

    public PhaseManager(PhaseData[] phaseDatas)
    {
        _phases = new Queue<PhaseData>(phaseDatas);
        // 항상 종료 페이즈를 꼬리로 추가
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

        if (_currentTurn == Team.All) // Team.All: 양 팀 모두 접수되어야 진행
        {
            _submittedInAll.Add(actingTeam);
            if (_submittedInAll.Contains(Team.Blue) && _submittedInAll.Contains(Team.Red))
            {
                Advance();
                _submittedInAll.Clear();
            }
        }
        else if (actingTeam == _currentTurn) Advance();
    }

    void Advance()
    {
        // 현재 페이즈의 큐가 비었으면 다음 페이즈로 이동
        if (_current.Phase.IsDone) _current = _phases.Dequeue();

        CurrentFlow = new GameFlowData(_current.GamePhase, _current.Phase.GetNext());
        OnFlowChanged?.Invoke(CurrentFlow);
    }
}
