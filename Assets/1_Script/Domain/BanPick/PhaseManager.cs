using System.Collections.Generic;

public enum GamePhase { Ban, Pick, Swap, Done }
public class PhaseManager
{
    Queue<GamePhase> phases = new();
    public PhaseManager(IReadOnlyList<GamePhase> phases)
    {
        this.phases = new Queue<GamePhase>(phases);
        CurrentPhase = this.phases.Dequeue();
    }

    public GamePhase CurrentPhase { get; private set; } = GamePhase.Ban;
    public void NextPhase(GamePhase phase) => CurrentPhase = phase; // Áö¶ö

    public GamePhase NextPhase()
    {
        if (phases.Count == 0) return GamePhase.Done;
        CurrentPhase = phases.Dequeue();
        return CurrentPhase;
    }
}
