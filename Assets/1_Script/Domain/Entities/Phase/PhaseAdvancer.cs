using System.Collections.Generic;

public enum GamePhase { Ban, Pick, Skill, Done }

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

public class PhaseAdvancer
{
    readonly Queue<PhaseData> _phases;
    PhaseData _current;

    public GameFlowData CurrentFlow { get; private set; }
    public Team CurrentTurn => CurrentFlow.Turn;

    public PhaseAdvancer(PhaseData[] phaseDatas)
    {
        _phases = new Queue<PhaseData>(phaseDatas);
        // Done은 마지막 고정
        _phases.Enqueue(new PhaseData(GamePhase.Done, new Phase(new[] { Team.All })));
    }

    public void Start()
    {
        _current = _phases.Dequeue();
        Advance();
    }

    public bool SubmitAction(Team actingTeam)
    {
        if (_current.GamePhase == GamePhase.Done) return false;

        if (actingTeam == CurrentTurn)
        {
            Advance();
            return true;
        }
        else throw new System.Exception($"현재 턴 {CurrentTurn}, 행동 팀 {actingTeam}");
    }

    void Advance()
    {
        if (_current == null || _current.Phase.IsDone) _current = _phases.Dequeue();
        ProgressFlow();
    }

    void ProgressFlow() => CurrentFlow = new GameFlowData(_current.GamePhase, _current.Phase.GetNext());
}
