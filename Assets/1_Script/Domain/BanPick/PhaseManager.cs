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
        CurrentPhaseData = this.phaseDatas.Dequeue();
    }

    public PhaseData CurrentPhaseData { get; private set; } = null;

    public GamePhase CurrentPhase => CurrentPhaseData.GamePhase;

    public bool Next()
    {
        if (CurrentPhaseData.GamePhase == GamePhase.Done) return false;

        CurrentPhaseData.Phase.Next();

        if (CurrentPhaseData.Phase.IsDone)
            CurrentPhaseData = phaseDatas.Dequeue();

        return true;
    }
}
