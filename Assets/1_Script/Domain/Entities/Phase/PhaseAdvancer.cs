using System.Collections.Generic;
using static Codice.CM.Common.CmCallContext;

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
    readonly Queue<PhaseData> phases;
    PhaseData current;

    public GameFlowData CurrentFlow { get; private set; }
    public Team CurrentTurn => CurrentFlow.Turn;

    public PhaseAdvancer(IEnumerable<PhaseData> phaseDatas)
    {
        phases = new Queue<PhaseData>(phaseDatas);
        // Done은 마지막 고정
        phases.Enqueue(new PhaseData(GamePhase.Done, new Phase(new[] { Team.All })));
        current = phases.Dequeue();
    }

    public void Start() => Advance();

    public bool TryAdvance(Team actingTeam)
    {
        if (current.GamePhase == GamePhase.Done) return false;

        if (actingTeam == CurrentTurn)
        {
            Advance();
            return true;
        }
        else throw new System.Exception($"현재 턴 {CurrentTurn}, 행동 팀 {actingTeam}");
    }

    void Advance()
    {
        if (current.Phase.IsDone) current = phases.Dequeue();
        CurrentFlow = new GameFlowData(current.GamePhase, current.Phase.GetNext());
    }
}
