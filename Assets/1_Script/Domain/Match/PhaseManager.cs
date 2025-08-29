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
    //readonly Queue<PhaseData> phaseDatas;
    //public PhaseManager(PhaseData[] phaseDatas)
    //{
    //    this.phaseDatas = new Queue<PhaseData>(phaseDatas);
    //    this.phaseDatas.Enqueue(new PhaseData(GamePhase.Done, new Phase(new Team[] { Team.All })));
    //}

    //PhaseData currentPhaseData = null;

    //GamePhase CurrentPhase => currentPhaseData.GamePhase;
    //public GameFlowData GetNextFlow()
    //{
    //    if(currentPhaseData == null || currentPhaseData.Phase.IsDone) 
    //        currentPhaseData = phaseDatas.Dequeue();

    //    if (CurrentPhase == GamePhase.Done) return new GameFlowData(GamePhase.Done, Team.All);

    //    return new GameFlowData(CurrentPhase, currentPhaseData.Phase.GetNext());
    //}

    private readonly Queue<PhaseData> _phases;
    private PhaseData _current;
    private Team _currentTurn;
    private readonly HashSet<Team> _submittedInAll = new();

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
                _submittedInAll.Clear();
                Advance();
            }
        }

        // 단일 팀 턴: 일치할 때만 진행
        if (actingTeam == _currentTurn) Advance();
    }

    void Advance()
    {
        // 현재 페이즈의 큐가 비었으면 다음 페이즈로 이동
        if (_current.Phase.IsDone) _current = _phases.Dequeue();

        _currentTurn = _current.Phase.GetNext();
        OnFlowChanged?.Invoke(new GameFlowData(_current.GamePhase, _currentTurn));
    }
}
