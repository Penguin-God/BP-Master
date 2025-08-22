using System.Collections.Generic;

public enum GamePhase { Ban, Pick, Swap, Done }

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
    readonly Queue<PhaseData> phaseDatas;
    public PhaseManager(PhaseData[] phaseDatas)
    {
        this.phaseDatas = new Queue<PhaseData>(phaseDatas);
        this.phaseDatas.Enqueue(new PhaseData(GamePhase.Done, new Phase(new Team[] { Team.All })));
    }

    public PhaseData CurrentPhaseData { get; private set; } = null;

    public GamePhase CurrentPhase => CurrentPhaseData.GamePhase;
    public Team CurrentTurn { get; private set; }

    public bool Next() // bool 리턴 절대 필요 없음
    {
        if (CurrentPhaseData.GamePhase == GamePhase.Done) return false;
        
        if (CurrentPhaseData.Phase.IsDone)
            CurrentPhaseData = phaseDatas.Dequeue();

        CurrentTurn = CurrentPhaseData.Phase.GetNext();

        return true;
    }

    public GameFlowData GetNextFlow()
    {
        if (CurrentPhaseData.GamePhase == GamePhase.Done) return new GameFlowData(GamePhase.Done, Team.All);

        if (CurrentPhaseData.Phase.IsDone)
            CurrentPhaseData = phaseDatas.Dequeue();

        CurrentTurn = CurrentPhaseData.Phase.GetNext();

        return new GameFlowData(CurrentPhase, CurrentTurn);
    }

    public void GameStart()
    {
        CurrentPhaseData = phaseDatas.Dequeue();
        CurrentTurn = CurrentPhaseData.Phase.GetNext();
    }
}
