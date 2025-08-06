

public enum BanPcikPhase { Ban = 0, Pick = 1, Swap = 2, Done = 3 }
public class PhaseManager
{
    public BanPcikPhase CurrentTurn { get; private set; } = BanPcikPhase.Ban;
    public BanPcikPhase Next()
    {
        if (CurrentTurn != BanPcikPhase.Done)
            CurrentTurn++;
        return CurrentTurn;
    }
}
