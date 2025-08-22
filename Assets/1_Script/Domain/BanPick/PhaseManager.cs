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

    PhaseData currentPhaseData = null;

    GamePhase CurrentPhase => currentPhaseData.GamePhase;
    public GameFlowData GetNextFlow()
    {
        if(currentPhaseData == null || currentPhaseData.Phase.IsDone) 
            currentPhaseData = phaseDatas.Dequeue();

        if (CurrentPhase == GamePhase.Done) return new GameFlowData(GamePhase.Done, Team.All);

        return new GameFlowData(CurrentPhase, currentPhaseData.Phase.GetNext());
    }
}
