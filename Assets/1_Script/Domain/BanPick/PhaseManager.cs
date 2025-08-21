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

    public bool Next()
    {
        if (CurrentPhaseData.GamePhase == GamePhase.Done) return false;
        
        UnityEngine.Debug.Log(CurrentPhaseData.GamePhase);
        UnityEngine.Debug.Log(CurrentPhaseData.Phase.IsDone);

        if (CurrentPhaseData.Phase.IsDone)
            CurrentPhaseData = phaseDatas.Dequeue();

        UnityEngine.Debug.Log(CurrentPhaseData.GamePhase);
        CurrentTurn = CurrentPhaseData.Phase.GetNext();

        return true;
    }

    public void GameStart()
    {
        CurrentTurn = CurrentPhaseData.Phase.PhaseStart();
        CurrentPhaseData = phaseDatas.Dequeue();
    }
}
