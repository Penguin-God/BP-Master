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
    Queue<GamePhase> _phases = new();
    
    public PhaseManager(IReadOnlyList<GamePhase> phases)
    {
        this._phases = new Queue<GamePhase>(phases);
        // CurrentPhase = this._phases.Dequeue();
    }

    readonly Queue<PhaseData> phaseDatas;
    public PhaseManager(PhaseData[] phaseDatas)
    {
        this.phaseDatas = new Queue<PhaseData>(phaseDatas);
        this.phaseDatas.Enqueue(new PhaseData(GamePhase.Done, null));
        CurrentPhaseData = this.phaseDatas.Dequeue();
    }

    public PhaseData CurrentPhaseData { get; private set; } = null;

    public GamePhase CurrentPhase;
    public GamePhase NextPhase()
    {
        if (_phases.Count == 0)
        {
            CurrentPhase = GamePhase.Done;
            return GamePhase.Done;
        }
        CurrentPhase = _phases.Dequeue();
        return CurrentPhase;
    }

    public bool Next()
    {
        if (CurrentPhaseData.GamePhase == GamePhase.Done) return false;

        CurrentPhaseData.Phase.Next();

        if (CurrentPhaseData.Phase.IsDone)
            CurrentPhaseData = phaseDatas.Dequeue();

        //if (CurrentPhaseData.Phase.IsDone)
        //    CurrentPhaseData = new PhaseData(GamePhase.Pick, new Phase(new Team[] { Team.Red, Team.Blue }));
        return true;
    }
}
